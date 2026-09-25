using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.CompanyJoinRequest;

namespace JobManagement.Application.Contracts.Services;

public interface ICompanyJoinRequestService : IGenericService
{
    Task<List<CompanyJoinRequestDto>> GetPagedAsync(PaginationDto dto);
    Task<CompanyJoinRequestDto> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateCompanyJoinRequestDto dto);
    Task AcceptJoinRequestAsync(ChangeCompanyJoinRequestStatusDto dto);
    Task DeleteAsync(int id);
    Task<List<CompanyJoinRequestDto>> SearchAsync(SearchDto dto);
    Task<List<CompanyJoinRequestDto>> SortAsync(SortDto dto);
}
