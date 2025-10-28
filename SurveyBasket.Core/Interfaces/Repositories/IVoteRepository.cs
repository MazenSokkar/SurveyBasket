using SurveyBasket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Core.Interfaces.Repositories
{
    public interface IVoteRepository
    {
        Task<Vote> SubmitVote(Vote vote, CancellationToken cancellationToken);
        Task<bool> QuestionHasVote(int pollId, string userId, CancellationToken cancellationToken = default);
    }
}
