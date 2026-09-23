using JobManagement.Application.DTOs.Common;
using JobManagement.Application.DTOs.Company;

namespace JobManagement.Application.Contracts.Services;

public interface ICompanyService : IGenericService
{
    Task<List<CompanyDto>> GetPagedAsync(PaginationDto dto);
    Task<CompanyDto> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateCompanyDto dto);
    Task UpdateAsync(UpdateCompanyDto dto);
    Task DeleteAsync(int id);
    Task AssignCompanyToUserAsync(AssignCompanyToUserDto dto);
    Task<List<CompanyDto>> SearchAsync(SearchDto dto);
    Task<List<CompanyDto>> SortAsync(SortDto dto);
}
