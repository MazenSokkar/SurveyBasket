using SurveyBasket.Contracts.Requests;
using SurveyBasket.Contracts.Responses;
using SurveyBasket.Core.Entities;

namespace SurveyBasket.Api.Mapping
{
    public static class PollMapping
    {

        public static Poll MapToPoll(this AddPollRequest poll)
        {
            return new () { 
                Title = poll.Title,
                Description = poll.Description 
            };
        }

        public static PollResponse MapToResponse(this Poll poll)
        {
            return new PollResponse {
                Id = poll.Id,
                Title = poll.Title,
                Description = poll.Description,
            };
        }

        public static IEnumerable<PollResponse> MapToResponse(this IEnumerable<Poll> polls)
        {
            return polls.Select(MapToResponse);
        }
    }
}
