using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Abstractions
{
    public record Error(string Code, string Description, int? statusCode)
    {
        public static readonly Error None = new(string.Empty, string.Empty, null);
    }
}
