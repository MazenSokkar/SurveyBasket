using FluentValidation;

namespace SurveyBasket.Contracts.Polls
{
    public class LoginRequestValidator : AbstractValidator<PollRequest>
    {

        public LoginRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.Summary)
                .NotEmpty()
                .Length(3, 1500);

            RuleFor(x => x.StartsAt)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(x => x.EndsAt)
                .NotEmpty();

            RuleFor(x => x)
                .Must(HasValidEndDate)
                .WithName(nameof(PollRequest.EndsAt))
                .WithMessage("{PropertyName} should be greater than or equal start date");

        }

        private bool HasValidEndDate(PollRequest request)
        {
            return request.EndsAt >= request.StartsAt;
        }
    }
}
