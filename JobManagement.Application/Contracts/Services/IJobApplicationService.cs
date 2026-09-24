using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.JobApplication;

namespace JobManagement.Application.Contracts.Services;

public interface IJobApplicationService : IGenericService
{
    Task<List<JobApplicationDto>> GetPagedAsync(PaginationDto dto);
    Task<List<JobApplicationDto>> GetApplications(int? jobId, PaginationDto dto);
    Task<JobApplicationDto> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateJobApplicationDto dto);
    Task ChangeStatus(ChangeJobApplicationStatusDto dto);
    Task DeleteAsync(int id);
    Task<List<JobApplicationDto>> SearchAsync(SearchDto dto);
    Task<List<JobApplicationDto>> SortAsync(SortDto dto);
}
