using JobManagement.Application.Models.Identity;

namespace JobManagement.Application.Contracts.Identity;

public interface IUserService
{
    Task<List<User>> GetUsersInRole(string role);
    Task<User> GetUser(string userId);
}
