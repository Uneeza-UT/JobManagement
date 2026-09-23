namespace JobManagement.Application.Contracts.Services;

public interface IGenericService
{
    Task<bool> GetUserRole(string role);
    Task<int> GetUserCompanyId();
    Task CheckUserAuthorization(int companyId, string message);
}
