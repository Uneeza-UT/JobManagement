using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.JobApplication;
using JobManagement.Domain;

namespace JobManagement.Application.Contracts.Persistence;

public interface IJobApplicationRepository : IGenericRepository<JobApplication>
{
    Task<List<JobApplication>> GetByUserIdAsync(string userId, int pageNumber = 1, int pageSize = 10);
    Task<List<JobApplication>> GetByJobIdAsync(int jobId, int pageNumber = 1, int pageSize = 10);
    Task<IReadOnlyList<JobApplication>> SearchAsync(string searchTerm);
    Task<bool> IsEmailUniqueAsync(string email, int jobId);
}