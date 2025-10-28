using Mapster;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Polls;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Infrastructure.Data;
using System.Security.Cryptography;

namespace SurveyBasket.Infrastructure.Repositories
{
    public class PollRepository(IGenericRepository<Poll> genericRepository, ApplicationDbContext context) : IPollRepository
    {

        private readonly IGenericRepository<Poll> _genericRepository = genericRepository;
        private readonly ApplicationDbContext _context = context;

        public async Task<Poll> AddAsync(Poll entity, CancellationToken cancellationToken = default)
            => await _genericRepository.AddAsync(entity, cancellationToken);

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
            => await _genericRepository.DeleteAsync(id, cancellationToken);

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) 
            => await _genericRepository.GetAllAsync(cancellationToken);

        public async Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
            => await _genericRepository.GetByIdAsync(id, cancellationToken);

        public async Task<bool> IsExistingTitleAsync(string title, CancellationToken cancellationToken)
            => await _context.Polls.AnyAsync(x => x.Title == title, cancellationToken);

        public async Task<bool> IsExistingTitleWithDifferentIdAsync(int id, string title, CancellationToken cancellationToken)
            => await _context.Polls.AnyAsync(x => x.Title == title && x.Id != id, cancellationToken);

        public async Task<bool> IsExistingPoll(int id, CancellationToken cancellationToken)
            => await _context.Polls.AnyAsync(x => x.Id == id, cancellationToken);

        public async Task<bool> IsRunningPoll(int id, CancellationToken cancellationToken)
            => await _context.Polls.AnyAsync(x => x.Id == id && x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);

        public async Task<IEnumerable<PollResponse>> GetCurrentAsync(CancellationToken cancellationToken)
            => await _context.Polls
                .Where(x => x.IsPublished && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow) && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                .AsNoTracking()
                .Select(p => new PollResponse(
                    p.Id,
                    p.Title,
                    p.Summary,
                    p.IsPublished,
                    p.StartsAt,
                    p.EndsAt
                ))
                .ToListAsync(cancellationToken);
    }
}
