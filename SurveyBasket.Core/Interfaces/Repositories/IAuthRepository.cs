using Microsoft.AspNetCore.Identity;
using SurveyBasket.Contracts.Auth;
using SurveyBasket.Core.Entities;

namespace SurveyBasket.Core.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);

        Task<ApplicationUser?> FindByIdAsync(string Id);

        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    }
}
