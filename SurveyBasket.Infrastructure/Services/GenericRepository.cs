using Microsoft.EntityFrameworkCore;
using SurveyBasket.Core.Interfaces;
using SurveyBasket.Infrastructure.Data;

namespace SurveyBasket.Infrastructure.Services
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {

        public readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(id, cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            return entity;
        }

        public bool UpdateAsync(int id, T entity, CancellationToken cancellationToken = default)
        {
            var t = _context.Set<T>().Find(id, cancellationToken);

            if (t != null)
            {
                t = entity;
                return true;
            }
            
            return false;
        }

        public bool Delete(int id)
        {
            var t = _context.Set<T>().Find(id);

            if(t != null)
            {
                _context.Set<T>().Remove(t);
                return true;
            }

            return false;
        }
    }
}
