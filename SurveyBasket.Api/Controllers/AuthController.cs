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
        public async Task<IActionResult> Login(LoginRequest authRequest, CancellationToken cancellationToken)
        {
            var authResult = await _authService.GetTokenAsync(authRequest.Email, authRequest.Password, cancellationToken);

            return authResult is null ? BadRequest("Invalid email or password") : Ok(authResult);
        }
    }
}
