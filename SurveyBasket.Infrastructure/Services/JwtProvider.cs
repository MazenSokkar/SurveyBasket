using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Services;
using SurveyBasket.Infrastructure.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SurveyBasket.Infrastructure.Services
{
    public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

      public (string token, int expiresIn) GenerateToken(ApplicationUser user)
        {
      Claim[] claims = [
     new(JwtRegisteredClaimNames.Sub, user.Id),
   new(JwtRegisteredClaimNames.Email, user.Email!),
      new(JwtRegisteredClaimNames.GivenName, user.FirstName),
      new(JwtRegisteredClaimNames.FamilyName, user.LastName),
      new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
   ];

  var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var expiresIn = _jwtOptions.ExpiryMinutes;

            var token = new JwtSecurityToken(
          issuer: _jwtOptions.Issuer,
          audience: _jwtOptions.Audience,
        claims: claims,
     expires: DateTime.UtcNow.AddMinutes(expiresIn),
              signingCredentials: signingCredentials
    );

    return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: expiresIn * 60);
        }
    }
}
