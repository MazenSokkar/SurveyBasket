using SurveyBasket.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials 
            = new("User.InvalidCredentials", "Invalid email or password");

        public static readonly Error InvalidToken
            = new("User.InvalidToken", "Invalid Token");
        
        public static readonly Error InvalidRefreshToken
            = new("User.InvalidRefreshToken", "Invalid Refresh Token");

        public static readonly Error UserNotFound
            = new("User.NotFound", "User Not Found");
    }
}
