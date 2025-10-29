using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Results
{
    public record VoteResponse
    (
        string VoterName,
        DateTime VoteDate,
        IEnumerable<QuestionAnswerResponse> SelectedAnswers
    );
}
