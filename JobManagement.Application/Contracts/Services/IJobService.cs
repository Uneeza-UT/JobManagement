using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.Company;
using JobManagement.Application.DTOs.Job;

namespace JobManagement.Application.Contracts.Services;

public interface IJobService : IGenericService
{
    Task<List<JobDto>> GetPagedAsync(PaginationDto dto);
    Task<JobDto> GetByIdAsync(int id);    
    Task<int> CreateAsync(CreateJobDto dto);
    Task UpdateAsync(UpdateJobDto dto);
    Task ChangeApprovalStatus(ChangeJobApprovalStatusDto dto);
    Task DeleteAsync(int id);
    Task<List<JobDto>> SearchAsync(SearchDto dto);
    Task<List<JobDto>> SortAsync(SortDto dto);
}
