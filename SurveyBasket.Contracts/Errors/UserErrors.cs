using Microsoft.AspNetCore.Http;
using SurveyBasket.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials 
            = new("User.InvalidCredentials", "Invalid email or password", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidToken
            = new("User.InvalidToken", "Invalid Token", StatusCodes.Status400BadRequest);
        
        public static readonly Error InvalidRefreshToken
            = new("User.InvalidRefreshToken", "Invalid Refresh Token", StatusCodes.Status400BadRequest);

        public static readonly Error UserNotFound
            = new("User.NotFound", "User Not Found", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedEmail
            = new("User.DuplicatedEmail", "Duplicated Email", StatusCodes.Status400BadRequest);

        public static readonly Error NotConfirmedAccount
            = new("User.NotConfirmedAccount", "Not Confirmed Account", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidCode
            = new("User.InvalidCode", "Invalid Code", StatusCodes.Status400BadRequest);

        public static readonly Error AlreadyConfirmed
            = new("User.AlreadyConfirmed", "Account is already confirmed", StatusCodes.Status400BadRequest);
    }
}
