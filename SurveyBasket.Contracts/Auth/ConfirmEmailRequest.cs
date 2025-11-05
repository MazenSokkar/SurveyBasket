namespace SurveyBasket.Contracts.Auth;

public record ConfirmEmailRequest
(
    string UserId,
    string VerificationCode
);
