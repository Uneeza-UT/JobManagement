using JobManagement.Application.Contracts.Services;
using JobManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobManagement.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        //Removes company IDs of all users belonging to a particular company from the user entity
        public async Task RemoveCompanyFromUsersAsync(int companyId)
        {
            var users = await _userManager.Users
                .Where(j => j.CompanyId == companyId)
                .ToListAsync();

            foreach (var user in users)
            {
                user.CompanyId = null;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
