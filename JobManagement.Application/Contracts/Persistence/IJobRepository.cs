using JobManagement.Application.DTOs.Job;
using JobManagement.Domain;

namespace JobManagement.Application.Contracts.Persistence;

public interface IJobRepository : IGenericRepository<Job>
{
    Task<IReadOnlyList<Job>> GetByCompanyIdAsync(int companyId, int pageNumber = 1, int pageSize = 10);
    Task<IReadOnlyList<Job>> SearchAsync(string searchTerm);
    Task<int> CountActiveJobsWithSameTitleAsync(string title, int companyId, int? excludeJobId = null);
    Task ExpireJobsAsync();
}