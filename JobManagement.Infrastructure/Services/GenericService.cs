using JobManagement.Application.Contracts.Services;
using JobManagement.Application.Exceptions;
using JobManagement.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace JobManagement.Infrastructure.Services
{
    public class GenericService : IGenericService
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly ICurrentUserService _currentUserService;

        public GenericService(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            this._userManager = userManager;
            this._currentUserService = currentUserService;
        }



        // Checks whether the current user has the specified role
        public async Task<bool> GetUserRole(string role)
        {
            var userId = _currentUserService.UserId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), userId);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            

            return userRoles.Contains(role);
        }



        //Fetches the company id associated with the user who is logged in  
        public async Task<int> GetUserCompanyId()
        {
            var userId = _currentUserService.UserId;

            var user = await _userManager.FindByIdAsync(userId);


            if (user == null)
            {
                throw new NotFoundException(nameof(ApplicationUser), userId);
            }


            //Checks if the user is associated with a company
            if (user.CompanyId == null)
            {
                throw new ForbiddenException("You are not authorized to perform this action because " +
                    "you are not associated with a company.");
            }


            return user.CompanyId.Value;
        }


        //Checks if user is authorized to perform a specific task
        public async Task CheckUserAuthorization(int companyId, string message)
        {
            var userompanyId = await GetUserCompanyId();

            if (userompanyId != companyId)
            {
                throw new ForbiddenException(message);
            }
        }
    }
}
