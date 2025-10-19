using FluentValidation;
using SurveyBasket.Contracts.Requests;

namespace SurveyBasket.Contracts.Validations
{
    public class AddPollRequestValidator : AbstractValidator<AddPollRequest>
    {

        public AddPollRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty();
        }

    }
}
