using System.Linq.Expressions;

namespace WebApp.Core.interfaces
{
    public interface IGenericRepo<T> where T : class
    {

        Task AddAsync(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null!, bool isTracked = true, Expression<Func<T, object>>[] include = null!);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null!, bool isTracked = true, int pageSize = 0,
            int pageNumber = 0, params Expression<Func<T, object>>[] includes);
        void Update(T entity);
        void Delete(T entity);


    }
}
