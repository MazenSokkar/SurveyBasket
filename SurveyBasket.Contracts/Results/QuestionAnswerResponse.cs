using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Results
{
    public record QuestionAnswerResponse
    (
        string Question,
        string Answer
    );
}
