using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces;

namespace SurveyBasket.Infrastructure.Services
{
    public class PollService(IUnitOfWork unitOfWork) : IPollService
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default) => await _unitOfWork.Polls.GetAllAsync(cancellationToken);

        public async Task<Poll?> GetByIdAsync(int Id, CancellationToken cancellationToken = default) => await _unitOfWork.Polls.GetByIdAsync(Id, cancellationToken);

        public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default)
        {
            await _unitOfWork.Polls.AddAsync(poll, cancellationToken);
            await _unitOfWork.Complete(cancellationToken);
            return poll;
        }

        public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default)
        {
            var currentPoll = await _unitOfWork.Polls.GetByIdAsync(id, cancellationToken);

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
            var result = await _unitOfWork.Polls.DeleteAsync(id);
            await _unitOfWork.Complete();
            return result;
        }

        public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _unitOfWork.Polls.GetByIdAsync(id, cancellationToken);

            if(poll is null)
                return false;

            poll.IsPublished = !poll.IsPublished;

            await _unitOfWork.Complete(cancellationToken);

            return true;
        }
    }
}
