using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Polls;
using SurveyBasket.Contracts.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Core.Interfaces.Services
{
    public interface IQuestionService
    {
        Task<Result<IEnumerable<QuestionResponse>>> GellQuestionsByPollIdAsync(int pollId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<QuestionResponse>>> GellAvailableQuestionsAsync(int pollId,string userId, CancellationToken cancellationToken = default);
        Task<Result<QuestionResponse>> GetQuestionsById(int pollId, int questionId, CancellationToken cancellationToken);
        Task<Result<QuestionResponse>> AddAsync(QuestionRequest questionRequest, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int questionId, QuestionRequest questionRequest, CancellationToken cancellationToken = default);
        Task<Result> ToggleActiveStatusAsync(int pollId, int questionId, CancellationToken cancellationToken = default);
    }
}
