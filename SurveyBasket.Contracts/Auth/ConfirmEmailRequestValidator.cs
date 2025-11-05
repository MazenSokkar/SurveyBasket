using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Auth;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.VerificationCode).NotEmpty();
    }
}
