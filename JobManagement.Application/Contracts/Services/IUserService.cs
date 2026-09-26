namespace JobManagement.Application.Contracts.Services;

public interface IUserService
{
    Task RemoveCompanyFromUsersAsync(int companyId);
}
