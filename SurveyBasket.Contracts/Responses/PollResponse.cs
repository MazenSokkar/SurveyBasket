using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Responses
{
    public class PollResponse
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = String.Empty;
    }
}
