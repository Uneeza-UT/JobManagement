using JobManagement.Domain;

namespace JobManagement.Application.Contracts.Persistence;

public interface ICompanyJoinRequestRepository : IGenericRepository<CompanyJoinRequest>
{
    Task<IReadOnlyList<CompanyJoinRequest>> GetByCompanyIdAsync(int companyId, int pageNumber = 1, int pageSize = 10);
    Task<IReadOnlyList<CompanyJoinRequest>> GetByUserIdAsync(string userId, int pageNumber = 1, int pageSize = 10);
    Task<bool> ExistsPendingRequest(int companyId, string userId);
    Task<IReadOnlyList<CompanyJoinRequest>> SearchAsync(string searchTerm);
}
