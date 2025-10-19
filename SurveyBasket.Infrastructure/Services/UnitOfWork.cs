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

        public async Task<int> Complete(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
