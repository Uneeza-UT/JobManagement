using JobManagement.Application.Contracts.Persistence;
using JobManagement.Domain;
using JobManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace JobManagement.Persistence.Repositories
{
    public class CompanyJoinRequestRepository : GenericRepository<CompanyJoinRequest>, ICompanyJoinRequestRepository
    {
        public CompanyJoinRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }


        //Get all requests sent to a company
        public async Task<IReadOnlyList<CompanyJoinRequest>> GetJoinRequestsByCompanyIdAsync(int companyId, int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<CompanyJoinRequest>()
               .AsNoTracking()
               .Where(j => j.CompanyId == companyId)
               .ToListAsync();
        }



        // Retrieves company entities that match the search term across the specified string properties.
        public async Task<IReadOnlyList<CompanyJoinRequest>> SearchAsync(string searchTerm)
        {
            return await _dbContext.Set<CompanyJoinRequest>()
                .AsNoTracking()
                .Where(j =>
                    j.FirstName.Contains(searchTerm) ||
                    j.LastName.Contains(searchTerm) ||
                    j.Email.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
