using JobManagement.Domain;
using System.Linq.Expressions;

namespace JobManagement.Application.Contracts.Persistence;

public interface ICompanyJoinRequestRepository : IGenericRepository<CompanyJoinRequest>
{
    Task<IReadOnlyList<CompanyJoinRequest>> GetByCompanyIdAsync(int companyId, int pageNumber = 1, int pageSize = 10);
    Task<IReadOnlyList<CompanyJoinRequest>> GetByUserIdAsync(string userId, int pageNumber = 1, int pageSize = 10);
    Task<bool> ExistsPendingRequest(int companyId, string userId);
    Task<IReadOnlyList<CompanyJoinRequest>> SearchForUserAsync(string searchTerm, string userId);
    Task<IReadOnlyList<CompanyJoinRequest>> SearchForCompanyAsync(string searchTerm, int companyId);
    Task<IReadOnlyList<CompanyJoinRequest>> SortJoinRequestsAsync<TKey>(
            bool descending,
            Expression<Func<CompanyJoinRequest, TKey>> keySelector,
            string? userId = null,
            int? companyId = null);
}
