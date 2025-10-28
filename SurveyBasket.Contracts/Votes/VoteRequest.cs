using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Votes
{
    public record VoteRequest
    (
        int PollId,
        IEnumerable<VoteAnswerRequest> Answers
    );
}
