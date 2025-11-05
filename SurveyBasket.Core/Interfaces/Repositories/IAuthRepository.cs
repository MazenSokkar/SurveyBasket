using Microsoft.AspNetCore.Identity;
using SurveyBasket.Core.Entities;

namespace SurveyBasket.Core.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<ApplicationUser?> FindByEmailAsync(string email);
    Task<ApplicationUser?> FindByIdAsync(string Id);
    Task<SignInResult> CheckPasswordAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
    Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
    Task<bool> CheckEmailAvailabilityAsync(string email);
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    Task<string> GenerateVerificationCode(ApplicationUser user);
    Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string code);
}
