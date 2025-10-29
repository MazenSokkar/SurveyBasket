using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Errors;
using SurveyBasket.Contracts.Results;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Infrastructure.Services
{
    public class ResultService(IResultRepository resultRepository, IPollRepository pollRepository) : IResultService
    {
        private readonly IResultRepository _resultRepository = resultRepository;
        private readonly IPollRepository _pollRepository = pollRepository;

        public async Task<Result<PollVoteResponse>> GetAllVotesAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var pollVote = await _resultRepository.GetAllVotesAsync(pollId, cancellationToken);

            if (pollVote is null)
                return Result.Failure<PollVoteResponse>(PollErrors.NotFoundPolls);

            return Result.Success(pollVote);
        }

        public async Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesCountPerDayAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var isExistPoll = await _pollRepository.IsExistingPoll(pollId, cancellationToken);

            if (!isExistPoll)
                return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.NotFoundPolls);

            var voterPerDay = await _resultRepository.GetVotesCountPerDayAsync(pollId, cancellationToken);

            return Result.Success(voterPerDay);
        }

        public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesCountPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
        {
            var isExistPoll = await _pollRepository.IsExistingPoll(pollId, cancellationToken);

            if (!isExistPoll)
                return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.NotFoundPolls);

            var votesPerQuestion = await _resultRepository.GetVotesCountPerQuestionAsync(pollId, cancellationToken);

            return Result.Success(votesPerQuestion);
        }
    }
}
