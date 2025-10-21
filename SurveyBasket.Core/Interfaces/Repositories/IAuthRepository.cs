using SurveyBasket.Contracts.Auth;

namespace SurveyBasket.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    }
}
