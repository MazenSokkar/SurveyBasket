using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Auth
{
    public record RefreshTokenRequest
    (
        string Token,
        string RefreshToken
    );
}
