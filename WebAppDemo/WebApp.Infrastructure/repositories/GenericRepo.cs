using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Core.interfaces;
using WebApp.Infrastructure.persistence;

namespace WebApp.Infrastructure.repositories
{
    public class GenericRepo<T> :IGenericRepo<T> where T : class
    {

        private readonly AppDbContext _context;
        private DbSet<T> _dbSet;
        public GenericRepo(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null!, bool isTracked = true
            , Expression<Func<T, object>>[] includes = null!)
        {
            var query = _dbSet.AsQueryable();
            if (!isTracked)
                query = query.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);
            if (includes != null)
            {
                foreach (var nav in includes)
                {
                    query = query.Include(nav);
                }
            }
            return (await query.FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null!, bool isTracked = true, int pageSize = 0,
            int pageNumber = 0, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();
            if (!isTracked)
                query = query.AsNoTracking();
            if (filter != null)
                query = query.Where(filter);

            if (includes.Length > 0)
            {
                foreach (var navProperty in includes)
                {
                    query = query.Include(navProperty);
                }
            }

            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            return (await query.ToListAsync())!;
        }

        public void Update(T entity) => _context.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);


    }
}
