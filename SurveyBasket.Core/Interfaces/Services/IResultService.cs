using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Core.Interfaces.Services
{
    public interface IResultService
    {
        Task<Result<PollVoteResponse>> GetAllVotesAsync(int pollId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<VotesPerDayResponse>>> GetVotesCountPerDayAsync(int pollId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<VotesPerQuestionResponse>>> GetVotesCountPerQuestionAsync(int pollId, CancellationToken cancellationToken = default);
    }
}
