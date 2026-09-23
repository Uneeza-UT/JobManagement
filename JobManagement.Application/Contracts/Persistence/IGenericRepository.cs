using JobManagement.Application.DTOs.Common;
using JobManagement.Domain;
using JobManagement.Domain.Shared;
using System.Linq.Expressions;

namespace JobManagement.Application.Contracts.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetPagedAsync(int pageNumber = 1, int pageSize = 10);
    Task<T> GetByIdAsync (int id);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<IReadOnlyList<T>> SortAsync<TKey>(bool descending, Expression<Func<T, TKey>> keySelector);  
}
