using JobManagement.Domain;

namespace JobManagement.Application.Contracts.Persistence;

public interface IJobApplicationRepository : IGenericRepository<JobApplication>
{
    Task<IReadOnlyList<JobApplication>> GetByUserIdAsync(string userId, int pageNumber = 1, int pageSize = 10);
    Task<IReadOnlyList<JobApplication>> GetByCompanyIdAsync(int companyId, int pageNumber = 1, int pageSize = 10);
    Task<JobApplication?> GetByIdWithJobAsync(int id);
    Task<IReadOnlyList<JobApplication>> SearchAsync(string searchTerm);
    Task<bool> IsEmailUniqueAsync(string email, int jobId);
}