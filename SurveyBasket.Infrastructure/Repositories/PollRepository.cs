using Microsoft.EntityFrameworkCore;
using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Infrastructure.Data;

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
    }
}
