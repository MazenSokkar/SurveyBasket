using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;

namespace SurveyBasket.Infrastructure.Repositories
{
    public class PollRepository(IGenericRepository<Poll> genericRepository) : IPollRepository
    {

        private readonly IGenericRepository<Poll> _genericRepository = genericRepository;

        public async Task<Poll> AddAsync(Poll entity, CancellationToken cancellationToken = default)
        {
            return await _genericRepository.AddAsync(entity, cancellationToken);    
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _genericRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _genericRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _genericRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _genericRepository.GetByIdAsync(id, cancellationToken);

            if (poll is null)
                return false;

            poll.IsPublished = !poll.IsPublished;

            return true;
        }

        public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default)
        {
            var currentPoll = await _genericRepository.GetByIdAsync(id, cancellationToken);

            if (currentPoll is null)
                return false;

            currentPoll.Title = poll.Title;
            currentPoll.Summary = poll.Summary;
            currentPoll.StartsAt = poll.StartsAt;
            currentPoll.EndsAt = poll.EndsAt;

            return true;
        }
    }
}
