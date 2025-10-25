using Microsoft.AspNetCore.Http;
using SurveyBasket.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Errors
{
    public static class QuestionErrors
    {
        public static readonly Error NotFoundQuestions
            = new("Questions.NotFound", "Question/s Not Found", StatusCodes.Status404NotFound);

        public static readonly Error DublicatedQuestion
            = new("Questions.DublicatedQuestion", "Another question with the same content already exists for this poll", StatusCodes.Status409Conflict);
    }
}
