using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Core.Interfaces.Repositories
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<bool> IsExistingQuestionForAdd(int pollId, string questionContent, CancellationToken cancellationToken);
        Task<bool> IsExistingQuestionForUpdate(int pollId, int questionId, string questionContent, CancellationToken cancellationToken);
        Task<IEnumerable<QuestionResponse>> GellQuestionsByPollId(int pollId, CancellationToken cancellationToken);
        Task<IEnumerable<QuestionResponse>> GetAvailableQuestions(int pollId, string UserId, CancellationToken cancellationToken);
        Task<Question?> GetQuestionsById(int pollId, int questionId, CancellationToken cancellationToken);
        Task<IEnumerable<int>> GetPollQuestionsIds(int pollId, CancellationToken cancellationToken);
    }
}
