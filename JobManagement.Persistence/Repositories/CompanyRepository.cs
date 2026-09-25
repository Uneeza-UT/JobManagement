using JobManagement.Application.Contracts.Persistence;
using JobManagement.Domain;
using JobManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace JobManagement.Persistence.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }


        //Get the company entity using user id
        public async Task<Company?> GetByUserIdAsync(string userId)
        {
            return await _dbContext.Set<Company>()
                .AsNoTracking()
                .FirstOrDefaultAsync(j => j.OwnerUserId == userId);
        }




        // Retrieves company entities that match the search term across the specified string properties.
        public async Task<IReadOnlyList<Company>> SearchAsync(string searchTerm)
        {
            return await _dbContext.Set<Company>()
                .AsNoTracking()
                .Where(j =>
                    j.Name.Contains(searchTerm) ||
                    j.Email.Contains(searchTerm) ||
                    j.PhoneNumber.Contains(searchTerm))
                .ToListAsync();
        }


        //Ensure that no other company is registered with the same email address
        public async Task<bool> IsCompanyEmailUniqueAsync(string email, int? excludeCompanyId = null)
        {
            return !await _dbContext.Companies
                .AnyAsync(j =>
                    j.Email == email &&
                    (!excludeCompanyId.HasValue || j.Id != excludeCompanyId.Value));
        }

    }
}
