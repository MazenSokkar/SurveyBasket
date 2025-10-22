using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Core.Interfaces.Services;
using SurveyBasket.Infrastructure.Repositories;

namespace SurveyBasket.Infrastructure.Services
{
    public class PollService(IPollRepository pollRepository, IUnitOfWork unitOfWork) : IPollService
    {
        private readonly IPollRepository _pollRepository = pollRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) 
            => await _pollRepository.GetAllAsync(cancellationToken);

        public async Task<Poll?> GetByIdAsync(int Id, CancellationToken cancellationToken = default) 
            => await _pollRepository.GetByIdAsync(Id, cancellationToken);

        public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default)
        {
            await _pollRepository.AddAsync(poll, cancellationToken);
            await _unitOfWork.Complete(cancellationToken);
            return poll;
        }

        public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default)
        {
            var currentPoll = await _pollRepository.GetByIdAsync(id, cancellationToken);

            if (currentPoll is null)
                return false;

            currentPoll.Title = poll.Title;
            currentPoll.Summary = poll.Summary;
            currentPoll.StartsAt = poll.StartsAt;
            currentPoll.EndsAt = poll.EndsAt;

            await _unitOfWork.Complete(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var result = await _pollRepository.DeleteAsync(id);
            await _unitOfWork.Complete(cancellationToken);
            return result;
        }

        public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _pollRepository.GetByIdAsync(id, cancellationToken);

            if (poll is null)
                return false;

            poll.IsPublished = !poll.IsPublished;

            await _unitOfWork.Complete(cancellationToken);
            return true;
        }
    }
}
