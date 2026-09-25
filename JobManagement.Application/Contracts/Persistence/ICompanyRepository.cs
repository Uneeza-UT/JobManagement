using JobManagement.Domain;

namespace JobManagement.Application.Contracts.Persistence;

public interface ICompanyRepository : IGenericRepository<Company>
{
    Task<Company?> GetByUserIdAsync(string userId);
    Task<IReadOnlyList<Company>> SearchAsync(string searchTerm);
    Task<bool> IsCompanyEmailUniqueAsync(string email, int? excludeCompanyId = null);
}