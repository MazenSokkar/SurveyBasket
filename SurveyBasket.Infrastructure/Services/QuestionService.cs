using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Errors;
using SurveyBasket.Contracts.Questions;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;

namespace SurveyBasket.Infrastructure.Services
{
    public class QuestionService(
        IQuestionRepository questionRepository,
        IUnitOfWork unitOfWork,
        IPollRepository pollRepository,
        IVoteRepository voteRepository,
        HybridCache hybridCache) : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository = questionRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPollRepository _pollRepository = pollRepository;
        private readonly IVoteRepository _voteRepository = voteRepository;
        private readonly HybridCache _hybridCache = hybridCache;
        private readonly string _cachePrefix = "availableQuestions";

        public async Task<Result<QuestionResponse>> AddAsync(QuestionRequest questionRequest, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _pollRepository.IsExistingPoll(questionRequest.PollId, cancellationToken);

            if (!pollIsExists)
                return Result.Failure<QuestionResponse>(PollErrors.NotFoundPolls);

            var questionIsExists = await _questionRepository.IsExistingQuestionForAdd(questionRequest.PollId, questionRequest.Content, cancellationToken);

            if (questionIsExists)
                return Result.Failure<QuestionResponse>(QuestionErrors.DublicatedQuestion);

            var newQuestion = questionRequest.Adapt<Question>();
            newQuestion.PollId = questionRequest.PollId;

            await _questionRepository.AddAsync(newQuestion, cancellationToken);
            await _unitOfWork.Complete(cancellationToken);

            var cacheKey = $"{_cachePrefix}-{questionRequest.PollId}";

            await _hybridCache.RemoveAsync(cacheKey, cancellationToken);

            return Result.Success(newQuestion.Adapt<QuestionResponse>());
        }

        public async Task<Result<QuestionResponse>> GetQuestionsById(int pollId, int questionId, CancellationToken cancellationToken)
        {
            var pollIsExists = await _pollRepository.IsExistingPoll(pollId, cancellationToken);

            if (!pollIsExists)
                return Result.Failure<QuestionResponse>(PollErrors.NotFoundPolls);

            var question = await _questionRepository.GetQuestionsById(pollId,questionId ,cancellationToken);

            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionErrors.NotFoundQuestions);

            var result = question.Adapt<QuestionResponse>();

            return Result.Success(result);
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GellQuestionsByPollIdAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _pollRepository.IsExistingPoll(pollId, cancellationToken);

            if (!pollIsExists)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.NotFoundPolls);

            var questions = await _questionRepository.GellQuestionsByPollId(pollId, cancellationToken);

            if (questions is null)
                return Result.Failure<IEnumerable<QuestionResponse>>(QuestionErrors.NotFoundQuestions);

            return Result.Success(questions);
        }

        public async Task<Result> ToggleActiveStatusAsync(int pollId, int questionId, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _pollRepository.IsExistingPoll(pollId, cancellationToken);

            if (!pollIsExists)
                return Result.Failure<QuestionResponse>(PollErrors.NotFoundPolls);

            var question = await _questionRepository.GetQuestionsById(pollId, questionId, cancellationToken);

            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionErrors.NotFoundQuestions);

            question.IsActive = !question.IsActive;

            await _unitOfWork.Complete(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(int questionId, QuestionRequest request, CancellationToken cancellationToken = default)
        {
            var pollIsExists = await _pollRepository.IsExistingPoll(request.PollId, cancellationToken);

            if (!pollIsExists)
                return Result.Failure<QuestionResponse>(PollErrors.NotFoundPolls);

            var isExcistingQuestion = await _questionRepository.IsExistingQuestionForUpdate(
                pollId: request.PollId,
                questionId: questionId,
                questionContent: request.Content,
                cancellationToken: cancellationToken
            );

            if (isExcistingQuestion)
                return Result.Failure<QuestionResponse>(QuestionErrors.DublicatedQuestion);
            
            var question = await _questionRepository.GetQuestionsById(request.PollId, questionId, cancellationToken);

            if (question is null)
                return Result.Failure<QuestionResponse>(QuestionErrors.NotFoundQuestions);

            question.PollId = request.PollId;
            question.Content = request.Content;

            // current answers
            var currentAnswers = question.Answers.Select(answer => answer.Content).ToList();

            //add new answers
            var newAnswers = request.Answers.Except(currentAnswers).ToList();

            newAnswers.ForEach(answer =>
            {
                question.Answers.Add(new Answer { Content = answer });
            });

            question.Answers.ToList().ForEach(answer =>
            {
                answer.IsActive = request.Answers.Contains(answer.Content);
            });

            await _unitOfWork.Complete(cancellationToken); 

            return Result.Success(question);
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GellAvailableQuestionsAsync(int pollId, string userId, CancellationToken cancellationToken = default)
        {
            var hasVote = await _voteRepository.QuestionHasVote(pollId, userId, cancellationToken);

            if (hasVote)
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);

            var isRunningPoll = await _pollRepository.IsRunningPoll(pollId, cancellationToken);
            
            if(!isRunningPoll)
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.NotFoundPolls);

            var cacheKey = $"{_cachePrefix}-{pollId}";

            var questions = await _hybridCache.GetOrCreateAsync<IEnumerable<QuestionResponse>>(
                cacheKey,
                async cacheEntry =>
                {
                    return await _questionRepository.GetAvailableQuestions(pollId, userId, cancellationToken);
                }
            );

            return Result.Success(questions);
        }
    }
}
