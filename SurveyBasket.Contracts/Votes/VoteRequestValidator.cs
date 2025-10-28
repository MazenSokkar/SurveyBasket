using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Votes
{
    public class VoteRequestValidator : AbstractValidator<VoteRequest>
    {
        public VoteRequestValidator()
        {
            RuleFor(x => x.Answers)
                .NotEmpty();

            RuleFor(x => x.PollId)
                .NotEmpty();

            RuleForEach(x => x.Answers)
                .SetInheritanceValidator(
                    v => v.Add(new VoteAnswerRequestValidator())
                );
        }
    }
}
