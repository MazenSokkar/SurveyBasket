using Microsoft.AspNetCore.Identity;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Auth;
using SurveyBasket.Contracts.Errors;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;
using System.Security.Cryptography;

namespace SurveyBasket.Infrastructure.Services
{
    public class AuthService(IAuthRepository authRepository, IJwtProvider jwtProvider) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        private readonly int _refreshTokenExpiryDays = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            // check user 
            var user = await _authRepository.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            // check password
            var isValidPassword = await _authRepository.CheckPasswordAsync(user, password);

            if (!isValidPassword)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

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

            return Result.Success<AuthResponse>(response);
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

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
