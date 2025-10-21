using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Core.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;

    }
}
