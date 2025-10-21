using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Auth
{
    public record LoginRequest(
        string Email,
        string Password
    );
}
