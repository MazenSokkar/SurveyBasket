using SurveyBasket.Contracts.Answers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Questions
{
    public record QuestionResponse
    (
        int Id,
        int PollId,
        bool IsActive,
        string Content,
        IEnumerable<AnswerResponse> Answers
    );
}
