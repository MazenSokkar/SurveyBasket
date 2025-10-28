using Microsoft.EntityFrameworkCore;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Infrastructure.Repositories
{
    public class VoteRepository(ApplicationDbContext applicationDbContext) : IVoteRepository
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task<bool> QuestionHasVote(int pollId, string userId, CancellationToken cancellationToken = default)
        {
            return await _applicationDbContext.Votes.AnyAsync(x => x.PollId == pollId && x.UserId == userId, cancellationToken);
        }

        public async Task<Vote> SubmitVote(Vote vote, CancellationToken cancellationToken)
        {
            var entry = await _applicationDbContext.Votes.AddAsync(vote, cancellationToken);
            return entry.Entity;
        }
    }
}
