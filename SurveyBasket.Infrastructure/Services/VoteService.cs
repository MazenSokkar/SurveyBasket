using Mapster;
using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Errors;
using SurveyBasket.Contracts.Votes;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;
using SurveyBasket.Infrastructure.Repositories;

namespace SurveyBasket.Infrastructure.Services;

public class VoteService(IPollRepository pollRepository, IVoteRepository voteRepository, IQuestionRepository questionRepository, IUnitOfWork unitOfWork) : IVoteService
{
    private readonly IPollRepository _pollRepository = pollRepository;
    private readonly IVoteRepository _voteRepository = voteRepository;
    private readonly IQuestionRepository _questionRepository = questionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> SubmitVoteAsync(string userId, VoteRequest voteRequest, CancellationToken cancellationToken)
    {
        var hasVotedBefore = await _voteRepository.QuestionHasVote(voteRequest.PollId, userId, cancellationToken);

        if (hasVotedBefore)
            return Result.Failure(VoteErrors.DuplicatedVote);

        var isPollRunning = await _pollRepository.IsRunningPoll(voteRequest.PollId,cancellationToken);

        if (!isPollRunning)
            return Result.Failure(PollErrors.NotFoundPolls);

        var pollAvailableQuestions = await _questionRepository.GetPollQuestionsIds(voteRequest.PollId, cancellationToken);

        if (!voteRequest.Answers.Select(x => x.QuestionId).SequenceEqual(pollAvailableQuestions))
            return Result.Failure(VoteErrors.InvalidQuestions);

        var vote = new Vote
        {
            PollId = voteRequest.PollId,
            UserId = userId,
            VoteAnswers = voteRequest.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
        };

        await _voteRepository.SubmitVote(vote, cancellationToken);

        await _unitOfWork.Complete(cancellationToken);

        return Result.Success();
    }
}
