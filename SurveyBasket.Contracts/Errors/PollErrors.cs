using SurveyBasket.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Errors
{
    public static class PollErrors
    {
        public static readonly Error NotFoundPolls
            = new ("Polls.NotFound", "Poll/s Not Found");
    }
}
