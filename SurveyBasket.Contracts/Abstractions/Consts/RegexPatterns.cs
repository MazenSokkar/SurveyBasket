namespace SurveyBasket.Contracts.Abstractions.Consts;

public static class RegexPatterns
{
    public const string Password = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*\-]).{8,}$";
}
