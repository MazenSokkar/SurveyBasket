namespace SurveyBasket.Contracts.Requests
{
    public record AddPollRequest (
        string Title, 
        string Summary,
        bool IsPublished,
        DateOnly StartsAt,
        DateOnly EndsAt
    );
}
