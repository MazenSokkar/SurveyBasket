using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Questions
{
    public record QuestionRequest
    (
        int PollId,
        string Content,
        List<string> Answers
    );
}
