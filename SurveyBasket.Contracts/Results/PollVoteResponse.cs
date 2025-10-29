
namespace SurveyBasket.Contracts.Results;

public record PollVoteResponse
(
  string Title,
  DateOnly StartsAt, 
  DateOnly EndsAt,
  IEnumerable<VoteResponse> Votes 
);
