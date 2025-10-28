using SurveyBasket.Contracts.Abstractions;
using SurveyBasket.Contracts.Votes;

namespace SurveyBasket.Core.Interfaces.Services;

public interface IVoteService
{
    Task<Result> SubmitVoteAsync(string userId, VoteRequest voteRequest, CancellationToken cancellationToken);
}
