using Microsoft.AspNetCore.Http;
using SurveyBasket.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Errors
{
    public static class PollErrors
    {
        public static readonly Error NotFoundPolls
            = new ("Polls.NotFound", "Poll/s Not Found", StatusCodes.Status404NotFound);

        public static readonly Error DublicatedPollTitle
            = new("Polls.DublicatedPollTitle", "Another poll with the same title already exists", StatusCodes.Status409Conflict);

    }
}
