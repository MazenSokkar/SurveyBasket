using FluentValidation;
using SurveyBasket.Contracts.Abstractions.Consts;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("password must have special chars, lower case, upper case and at least 8 digits");
        
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3,100);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3,100);
    }
}
