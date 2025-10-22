using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;

namespace SurveyBasket.Infrastructure.Repositories
{
    public class PollRepository(IGenericRepository<Poll> genericRepository) : IPollRepository
    {

        private readonly IGenericRepository<Poll> _genericRepository = genericRepository;

        public async Task<Poll> AddAsync(Poll entity, CancellationToken cancellationToken = default)
            => await _genericRepository.AddAsync(entity, cancellationToken);    

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
            => await _genericRepository.DeleteAsync(id, cancellationToken);

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) 
            => await _genericRepository.GetAllAsync(cancellationToken);

        public async Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken = default) 
            => await _genericRepository.GetByIdAsync(id, cancellationToken);
    }
}
