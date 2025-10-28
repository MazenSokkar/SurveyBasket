using Microsoft.AspNetCore.Http;
using SurveyBasket.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Errors
{
    public static class VoteErrors
    {
        public static readonly Error NotFoundVotes
           = new("Votes.NotFound", "Vote/s Not Found", StatusCodes.Status404NotFound);

        public static readonly Error DuplicatedVote =
            new("Vote.DuplicatedVote", "Poll is voted once with the same user", StatusCodes.Status409Conflict);

        public static readonly Error InvalidQuestions =
            new("Vote.InvalidQuestions", "InvalidQuestions", StatusCodes.Status400BadRequest);
    }
}
