using FluentValidation;
using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Contracts.Validations
{
    public class AddPollRequestValidator : AbstractValidator<AddPollRequest>
    {

        public AddPollRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.Summary)
                .NotEmpty()
                .Length(3, 1500);
        }

    }
}
