using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Results
{
    public record VotesPerQuestionResponse
    (
        string Question,
        IEnumerable<VotesPerAnswersResponse> SelectedAnswers
    );
}
