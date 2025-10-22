using Microsoft.AspNetCore.Identity;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Auth;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;
using System.Security.Cryptography;

namespace SurveyBasket.Infrastructure.Services
{
    public class AuthService(IAuthRepository authRepository, IJwtProvider jwtProvider) : IAuthService
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        private readonly int _refreshTokenExpiryDays = 14;

        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            // check user 
            var user = await _authRepository.FindByEmailAsync(email);

            if (user is null)
            {
                return null;
            }

            // check password
            var isValidPassword = await _authRepository.CheckPasswordAsync(user, password);

            if (!isValidPassword)
            {
                return null;
            }

            // generate token
            var (token, expiresIn) = _jwtProvider.GenerateToken(user);

            // generate refresh token
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiraton = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            // save the refresh token to the database
            user.RefreshTokens.Add(
                new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpiraton
                }
            );
            await _authRepository.UpdateUserAsync(user);

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiraton);
        }

        public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);

            if (userId is null)
                return null;

            var user = await _authRepository.FindByIdAsync(userId);

            if (user is null)
                return null;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return null;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            // generate token
            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);

            // generate refresh token
            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiraton = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

            // save the refresh token to the database
            user.RefreshTokens.Add(
                new RefreshToken
                {
                    Token = newRefreshToken,
                    ExpiresOn = refreshTokenExpiraton
                }
            );
            await _authRepository.UpdateUserAsync(user);

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiraton);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);

            if (userId is null)
                return false;

            var user = await _authRepository.FindByIdAsync(userId);

            if (user is null)
                return false;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

            if (userRefreshToken is null)
                return false;

            userRefreshToken.RevokedOn = DateTime.UtcNow;

            await _authRepository.UpdateUserAsync(user);

            return true;
        }

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
