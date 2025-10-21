using SurveyBasket.Core.Entities;

namespace SurveyBasket.Core.Interfaces.Services
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user);
    }
}
