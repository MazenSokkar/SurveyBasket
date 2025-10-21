using SurveyBasket.Contracts.Auth;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;

namespace SurveyBasket.Infrastructure.Services
{
    public class AuthService(IAuthRepository authRepository) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;

        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            return await _authRepository.GetTokenAsync(email, password, cancellationToken);
        }

        public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _authRepository.GetRefreshTokenAsync(token, refreshToken, cancellationToken);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _authRepository.RevokeRefreshTokenAsync(token, refreshToken, cancellationToken);
        }
    }
}
