namespace SurveyBasket.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        bool UpdateAsync(int id, T entity, CancellationToken cancellationToken = default);

        bool Delete(int id);
    }
}
