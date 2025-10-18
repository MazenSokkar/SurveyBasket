using Microsoft.EntityFrameworkCore;
using SurveyBasket.Core.Interfaces;
using SurveyBasket.Infrastructure.Data;

namespace SurveyBasket.Infrastructure.Services
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {

        public readonly ApplicationDbContext _context = context;

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public bool Update(int id, T entity)
        {
            var t = _context.Set<T>().Find(id);

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
