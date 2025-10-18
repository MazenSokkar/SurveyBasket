namespace SurveyBasket.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        T Add(T entity);

        bool Update(int id, T entity);

        bool Delete(int id);
    }
}
