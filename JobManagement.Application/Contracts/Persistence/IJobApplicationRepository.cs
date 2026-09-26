using JobManagement.Domain;
using System.Linq.Expressions;

namespace JobManagement.Application.Contracts.Persistence;

public interface IJobApplicationRepository : IGenericRepository<JobApplication>
{
    Task<IReadOnlyList<JobApplication>> GetByUserIdAsync(string userId, int pageNumber = 1, int pageSize = 10);
    Task<IReadOnlyList<JobApplication>> GetByCompanyIdAsync(int companyId, int pageNumber = 1, int pageSize = 10);
    Task<JobApplication?> GetByIdWithJobAsync(int id);
    Task<IReadOnlyList<JobApplication>> SearchForUserAsync(string searchTerm, string userId);
    Task<IReadOnlyList<JobApplication>> SearchForCompanyAsync(string searchTerm, int companyId);
    Task<IReadOnlyList<JobApplication>> SortJobApplicationsAsync<TKey>(
            bool descending,
            Expression<Func<JobApplication, TKey>> keySelector,
            string? userId = null,
            int? companyId = null);
    Task<bool> IsEmailUniqueAsync(string email, int jobId);
}