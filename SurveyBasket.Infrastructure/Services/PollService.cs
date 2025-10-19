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

        //public bool Update(int id, Poll poll)
        //{
        //    var result = _unitOfWork.Polls.Update(id, poll);
        //    if (result)
        //        _unitOfWork.Complete();
        //    return result;
        //}

        //public bool Delete(int id)
        //{
        //    var result = _unitOfWork.Polls.Delete(id);
        //    if (result)
        //        _unitOfWork.Complete();
        //    return result;
        //}
    }
}
