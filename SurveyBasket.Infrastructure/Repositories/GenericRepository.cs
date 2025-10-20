using Microsoft.EntityFrameworkCore;
using SurveyBasket.Core.Interfaces.Repositories;
using SurveyBasket.Infrastructure.Data;

namespace SurveyBasket.Infrastructure.Repositories
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

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var t = await GetByIdAsync(id, cancellationToken);

            if(t != null)
            {
                _context.Set<T>().Remove(t);
                return true;
            }

            return false;
        }
    }
}
