using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Contracts.Auth;
using SurveyBasket.Core.Interfaces.Services;

namespace SurveyBasket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest authRequest, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetTokenAsync(authRequest.Email, authRequest.Password, cancellationToken);

            return authResult is null ? BadRequest("Invalid email or password") : Ok(authResult);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {
            var result = await _authService.GetRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);

            return result is null ? BadRequest("Invalid Token") : Ok(result);
        }

        [HttpPost("RevokeRefreshToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
        {
            var result = await _authService.RevokeRefreshTokenAsync(refreshTokenRequest.Token, refreshTokenRequest.RefreshToken, cancellationToken);

            return result is false ? BadRequest("Operation failed") : Ok();
        }
    }
}
