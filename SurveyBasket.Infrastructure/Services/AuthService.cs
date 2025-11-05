using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Auth;
using SurveyBasket.Contracts.Errors;
using SurveyBasket.Contracts.Helpers;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace SurveyBasket.Infrastructure.Services;

public class AuthService(IAuthRepository authRepository,
    IJwtProvider jwtProvider,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly IAuthRepository _authRepository = authRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    
    private readonly int _refreshTokenExpiryDays = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        // check user 
        var user = await _authRepository.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        // check password
        var result = await _authRepository.CheckPasswordAsync(user, password, false, false);

        if (result.Succeeded)
        {
            // generate token
            var (token, expiresIn) = _jwtProvider.GenerateToken(user);

            // generate refresh token
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiraton = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            // save the refresh token to the database
            user.RefreshTokens.Add(
                new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiraton
                }
            );
            await _authRepository.UpdateUserAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiraton);

            return Result.Success(response);
        }

        return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.NotConfirmedAccount : UserErrors.InvalidCredentials);
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var user = await _authRepository.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        // generate token
        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);

        // generate refresh token
        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiraton = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        // save the refresh token to the database
        user.RefreshTokens.Add(
            new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiraton
            }
        );
        await _authRepository.UpdateUserAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiraton);

        return Result.Success(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        var user = await _authRepository.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _authRepository.UpdateUserAsync(user);

        return Result.Success();
    }
    
    public async Task<Result> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _authRepository.CheckEmailAvailabilityAsync(registerRequest.Email);

        if (emailIsExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var user = registerRequest.Adapt<ApplicationUser>();

        var result = await _authRepository.CreateUserAsync(user, registerRequest.Password);

        if(result.Succeeded)
        {
            var code = await _authRepository.GenerateVerificationCode(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            await SendConfirmationEmail(user, code);

            return Result.Success();
        }

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }


    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
    {
        var user = await _authRepository.FindByIdAsync(confirmEmailRequest.UserId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidCode);

        if(user.EmailConfirmed)
            return Result.Failure(UserErrors.AlreadyConfirmed);

        var code = confirmEmailRequest.VerificationCode;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await _authRepository.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {
            return Result.Success();
        }

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> ResendConfirmEmailAsync(ResendConfirmationEmailRequest request)
    {
        var user = await _authRepository.FindByEmailAsync(request.Email);

        if(user is null)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.AlreadyConfirmed);

        var code = await _authRepository.GenerateVerificationCode(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        await SendConfirmationEmail(user, code);

        return Result.Success();
    }

    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                    { "{{name}}", user.FirstName },
                    { "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&verificationCode={code}" }
            }
        );

        await _emailSender.SendEmailAsync(user.Email!, "SurveyBasket Email Confirmation", emailBody);
    }
}
