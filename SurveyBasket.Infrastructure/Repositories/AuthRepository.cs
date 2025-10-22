using Microsoft.AspNetCore.Identity;
using SurveyBasket.Contracts.Auth;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;
using System.Security.Cryptography;

namespace SurveyBasket.Infrastructure.Repositories
{
    public class AuthRepository(UserManager<ApplicationUser> userManager) : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<ApplicationUser?> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        public async Task<ApplicationUser?> FindByIdAsync(string Id) => await _userManager.FindByIdAsync(Id);
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password) => await _userManager.CheckPasswordAsync(user, password);
        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user) => await _userManager.UpdateAsync(user);
    }
}
