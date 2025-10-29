using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SurveyBasket.Contracts.Results;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyBasket.Infrastructure.Repositories
{
    public class ResultRepository(ApplicationDbContext context) : IResultRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<PollVoteResponse> GetAllVotesAsync(int pollId, CancellationToken cancellationToken = default)
        {
            return await _context.Polls
                .Where(x => x.Id ==  pollId)
                .Select(x => new PollVoteResponse(
                    x.Title,
                    x.StartsAt,
                    x.EndsAt,
               x.Votes.Select(v => new VoteResponse(
                  $"{v.User.FirstName} {v.User.LastName}",
                    v.SubmittedOn,
                v.VoteAnswers.Select(a => new QuestionAnswerResponse(
                            a.Question.Content,
                            a.Answer.Content
                            ))
                    ))
                ))
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<VotesPerDayResponse>> GetVotesCountPerDayAsync(int pollId, CancellationToken cancellationToken = default)
        {
            return await _context.Votes
                .Where(x => x.PollId == pollId)
                .GroupBy(x => new { Date = DateOnly.FromDateTime(x.SubmittedOn) })
                .Select(g => new VotesPerDayResponse
                (
                    g.Key.Date,
                    g.Count()
                ))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<VotesPerQuestionResponse>> GetVotesCountPerQuestionAsync(int pollId, CancellationToken cancellationToken = default)
        {
            return await _context.VoteAnswers
                .Where(x => x.Vote.PollId == pollId)
                .Select(x => new VotesPerQuestionResponse
                (
                    x.Question.Content,
                    x.Question.Votes
                        .GroupBy( x => new { AnswerId = x.Answer.Id, AnswerContent = x.Answer.Content })
                        .Select(g => new VotesPerAnswersResponse
                        (
                            g.Key.AnswerContent, g.Count()
                        ))
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
