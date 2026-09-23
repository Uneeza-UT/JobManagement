using JobManagement.Application.Contracts.Persistence;
using JobManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace JobManagement.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        //Retrieve the paginated list of database entities
        public async Task<IReadOnlyList<T>> GetPagedAsync(int pageNumber = 1, int pageSize = 10)
        {
            int skip = (pageNumber - 1) * pageSize;
            return await _dbContext.Set<T>()
                .AsNoTracking()
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }


        // Retrieves entities sorted according to the specified sort option.
        public async Task<IReadOnlyList<T>> SortAsync<TKey>(bool descending, Expression<Func<T, TKey>> keySelector)
        {
            var query = _dbContext.Set<T>().AsNoTracking();
            query = descending ? query.OrderByDescending(keySelector) : query.OrderBy(keySelector);
            return await query.ToListAsync();
        }
    }
}
