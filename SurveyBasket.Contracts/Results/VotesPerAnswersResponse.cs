using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Results
{
    public record VotesPerAnswersResponse
    (
        string Answer,
        int Count
    );
}
