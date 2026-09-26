using JobManagement.Application.Contracts.Persistence;
using JobManagement.Domain;
using JobManagement.Domain.Enums;
using JobManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobManagement.Persistence.Repositories
{
    public class CompanyJoinRequestRepository : GenericRepository<CompanyJoinRequest>, ICompanyJoinRequestRepository
    {
        public CompanyJoinRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }


        //Get all requests sent by a user
        public async Task<IReadOnlyList<CompanyJoinRequest>> GetByUserIdAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<CompanyJoinRequest>()
               .AsNoTracking()
               .Where(j => j.UserId == userId)
               .ToListAsync();
        }



        //Get all requests sent to a company
        public async Task<IReadOnlyList<CompanyJoinRequest>> GetByCompanyIdAsync(int companyId, int pageNumber = 1, int pageSize = 10)
        {
            return await _dbContext.Set<CompanyJoinRequest>()
               .AsNoTracking()
               .Where(j => j.CompanyId == companyId)
               .ToListAsync();
        }





        //Check if the request already exists for the same company
        public async Task<bool> ExistsPendingRequest(int companyId, string userId)
        {
            return await _dbContext.CompanyJoinRequests
                .AnyAsync(j => 
                    j.UserId == userId &&
                    j.CompanyId == companyId && 
                    j.Status == JoinRequestStatus.Pending);
        }




        // Retrieves the join requests sent by a user that match the search term across the specified string properties.
        public async Task<IReadOnlyList<CompanyJoinRequest>> SearchForUserAsync(string searchTerm, string userId)
        {
            return await _dbContext.Set<CompanyJoinRequest>()
                .AsNoTracking()
                .Where(j =>
                    j.UserId == userId &&
                    (j.FirstName.Contains(searchTerm) ||
                    j.LastName.Contains(searchTerm) ||
                    j.Email.Contains(searchTerm)))
                .ToListAsync();
        }



        // Retrieves the join requests sent to a company that match the search term across the specified string properties.
        public async Task<IReadOnlyList<CompanyJoinRequest>> SearchForCompanyAsync(string searchTerm, int companyId)
        {
            return await _dbContext.Set<CompanyJoinRequest>()
                .AsNoTracking()
                .Where(j =>
                    j.CompanyId == companyId &&
                    (j.FirstName.Contains(searchTerm) ||
                    j.LastName.Contains(searchTerm) ||
                    j.Email.Contains(searchTerm)))
                .ToListAsync();
        }



        public async Task<IReadOnlyList<CompanyJoinRequest>> SortJoinRequestsAsync<TKey>(
            bool descending,
            Expression<Func<CompanyJoinRequest, TKey>> keySelector,
            string? userId = null,
            int? companyId = null)
        {
            var query = _dbContext.CompanyJoinRequests.AsNoTracking();

            if (userId != null)
            {
                query = query.Where(x => x.UserId == userId);
            }

            else if (companyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == companyId);
            }

            query = descending
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);

            return await query.ToListAsync();
        }

    }
}
