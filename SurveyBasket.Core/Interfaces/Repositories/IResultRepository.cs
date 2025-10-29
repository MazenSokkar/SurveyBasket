using SurveyBasket.Contracts.Results;
using System.Collections.Generic;

namespace SurveyBasket.Core.Interfaces.Repositories
{
    public interface IResultRepository
    {
        Task<PollVoteResponse> GetAllVotesAsync(int pollId, CancellationToken cancellationToken = default);
        Task<IEnumerable<VotesPerDayResponse>> GetVotesCountPerDayAsync(int pollId, CancellationToken cancellationToken = default);
        Task<IEnumerable<VotesPerQuestionResponse>> GetVotesCountPerQuestionAsync(int pollId, CancellationToken cancellationToken = default);
    }
}
