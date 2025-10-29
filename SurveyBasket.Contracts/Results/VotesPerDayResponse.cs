using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Results
{
    public record VotesPerDayResponse
    (
        DateOnly Date,
        int NumberOfVotes
    );
}
