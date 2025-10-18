using SurveyBasket.Core.Entities;
using SurveyBasket.Core.Interfaces;
using SurveyBasket.Infrastructure.Services;
using SurveyBasket.Infrastructure.Data;

namespace SurveyBasket.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {

        public readonly ApplicationDbContext _context;

        public IGenericRepository<Poll> Polls {  get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

           Polls = new GenericRepository<Poll>(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
