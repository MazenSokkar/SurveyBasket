using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Auth;

namespace SurveyBasket.Core.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest);
    Task<Result> ResendConfirmEmailAsync(ResendConfirmationEmailRequest resendConfirmationEmailRequest);
}
