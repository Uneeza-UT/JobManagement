using JobManagement.Domain;

namespace JobManagement.Application.Contracts.Persistence;

public interface ICompanyRepository : IGenericRepository<Company>
{
    Task<IReadOnlyList<Company>> SearchAsync(string searchTerm);
    Task<bool> IsCompanyEmailUniqueAsync(string email, int? excludeCompanyId = null);
}