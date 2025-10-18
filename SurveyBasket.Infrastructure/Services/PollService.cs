using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces;

namespace SurveyBasket.Infrastructure.Services
{
    public class PollService(IUnitOfWork unitOfWork) : IPollService
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public IEnumerable<Poll> GetAll() => _unitOfWork.Polls.GetAll();

        public Poll GetById(int Id) => _unitOfWork.Polls.GetById(Id);

        public Poll Add(Poll poll)
        {
            _unitOfWork.Polls.Add(poll);
            return poll;
        }

        public bool Update(int id, Poll poll)
        {
            return _unitOfWork.Polls.Update(id, poll);
        }

        public bool Delete(int id)
        {
            return _unitOfWork.Polls.Delete(id);
        }
    }
}
