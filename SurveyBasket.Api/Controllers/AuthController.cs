using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Auth;
using SurveyBasket.Core.Interfaces.Services;
using LoginRequest = SurveyBasket.Contracts.Auth.LoginRequest;
using RegisterRequest = SurveyBasket.Contracts.Auth.RegisterRequest;
using ResendConfirmationEmailRequest = SurveyBasket.Contracts.Auth.ResendConfirmationEmailRequest;

namespace SurveyBasket.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest authRequest, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(authRequest.Email, authRequest.Password, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value)
            : authResult.ToProblem();
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
    {
        var result = await _authService.GetRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) 
            : result.ToProblem();
    }

    [HttpPost("RevokeRefreshToken")]
    public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);

        return result.IsSuccess ? Ok(result.IsSuccess)
            : result.ToProblem();
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken)
    {
        var result = await _authService.RegisterAsync(registerRequest, cancellationToken);

        return result.IsSuccess ? Ok(result.IsSuccess)
            : result.ToProblem();
    }
    
    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest confirmEmailRequest)
    {
        var result = await _authService.ConfirmEmailAsync(confirmEmailRequest);

        return result.IsSuccess ? Ok(result.IsSuccess)
            : result.ToProblem();
    }

    [HttpPost("ResendConfirmationEmail")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest resendConfirmationEmailRequest)
    {
        var result = await _authService.ResendConfirmEmailAsync(resendConfirmationEmailRequest);

        return result.IsSuccess ? Ok(result.IsSuccess)
            : result.ToProblem();
    }
}
