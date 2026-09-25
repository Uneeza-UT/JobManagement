using JobManagement.Domain;

namespace JobManagement.Application.Contracts.Persistence;

public interface ICompanyJoinRequestRepository : IGenericRepository<CompanyJoinRequest>
{
    Task<IReadOnlyList<CompanyJoinRequest>> GetJoinRequestsByCompanyIdAsync(int companyId, int pageNumber = 1, int pageSize = 10);
    Task<IReadOnlyList<CompanyJoinRequest>> SearchAsync(string searchTerm);
}
