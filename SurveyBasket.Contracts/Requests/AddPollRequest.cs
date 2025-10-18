using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Contracts.Requests
{
    public class AddPollRequest
    {

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = String.Empty;

    }
}
