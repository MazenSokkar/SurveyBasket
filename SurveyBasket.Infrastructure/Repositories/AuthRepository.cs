using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Auth;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;

namespace SurveyBasket.Infrastructure.Repositories;

public class AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<ApplicationUser?> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
    public async Task<ApplicationUser?> FindByIdAsync(string Id) => await _userManager.FindByIdAsync(Id);
    public async Task<SignInResult> CheckPasswordAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure) => await _signInManager.PasswordSignInAsync(user,password,isPersistent,lockoutOnFailure);
    public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user) => await _userManager.UpdateAsync(user);
    public async Task<bool> CheckEmailAvailabilityAsync(string email)
        => await _userManager.Users.AnyAsync(u => u.Email == email);
    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        => await _userManager.CreateAsync(user, password);
    public async Task<string> GenerateVerificationCode(ApplicationUser user)
        => await _userManager.GenerateEmailConfirmationTokenAsync(user);
    public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string code)
        => await _userManager.ConfirmEmailAsync(user, code);
}
