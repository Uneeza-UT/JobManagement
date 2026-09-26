using JobManagement.Application.Contracts.Persistence;
using JobManagement.Domain;
using JobManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobManagement.Persistence.Repositories
{
    public class JobApplicationRepository : GenericRepository<JobApplication>, IJobApplicationRepository
    {
        public JobApplicationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }


        // Retrieves job applications submitted by a particular student.
        public async Task<IReadOnlyList<JobApplication>> GetByUserIdAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            int skip = (pageNumber - 1) * pageSize;

            return await _dbContext.Set<JobApplication>()
                .AsNoTracking()
                .Where(j =>
                    j.ApplicantId == userId)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }



        // Retrieves job applications submitted to a particular company.
        public async Task<IReadOnlyList<JobApplication>> GetByCompanyIdAsync(int companyId, int pageNumber = 1, int pageSize = 10)
        {
            int skip = (pageNumber - 1) * pageSize;

            return await _dbContext.Set<JobApplication>()
                .AsNoTracking()
                .Where(j =>
                    j.Job!.CompanyId == companyId)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }


        // Retrieves a job application including job entity.
        public async Task<JobApplication?> GetByIdWithJobAsync(int id)
        {
            return await _dbContext.Set<JobApplication>()
                .AsNoTracking()
                .Include(j => j.Job)
                .FirstOrDefaultAsync(j => j.Id == id);
        }



        //Ensure that no other applicant has applied with the same email address for a job
        public async Task<bool> IsEmailUniqueAsync(string email, int jobId)
        {
            return !await _dbContext.JobApplications
                .AnyAsync(j =>
                    j.JobId == jobId &&
                    j.Email == email);
        }


        // Retrieves job applications of an applicant that match the search term across the specified string properties.
        public async Task<IReadOnlyList<JobApplication>> SearchForUserAsync(string searchTerm, string userId)
        {
            return await _dbContext.Set<JobApplication>()
                .AsNoTracking()
                .Where(j =>
                    j.ApplicantId == userId &&
                    (j.FirstName.Contains(searchTerm) ||
                    j.LastName.Contains(searchTerm) ||
                    j.Email.Contains(searchTerm) ||
                    j.PhoneNumber.Contains(searchTerm) ||
                    j.Job!.Title.Contains(searchTerm)))
                .ToListAsync();
        }



        // Retrieves job application for jobs posted by a particular company
        // that match the search term across the specified string properties
        public async Task<IReadOnlyList<JobApplication>> SearchForCompanyAsync(string searchTerm, int companyId)
        {
            return await _dbContext.Set<JobApplication>()
                .AsNoTracking()
                .Where(j =>
                    j.Job!.CompanyId == companyId &&
                    (j.FirstName.Contains(searchTerm) ||
                    j.LastName.Contains(searchTerm) ||
                    j.Email.Contains(searchTerm) ||
                    j.PhoneNumber.Contains(searchTerm) ||
                    j.Job!.Title.Contains(searchTerm)))
                .ToListAsync();
        }



        public async Task<IReadOnlyList<JobApplication>> SortJobApplicationsAsync<TKey>(
            bool descending, 
            Expression<Func<JobApplication, TKey>> keySelector,
            string? userId = null, 
            int? companyId = null)
        {
            var query = _dbContext.JobApplications.AsNoTracking();

            if (userId != null)
            {
                query = query.Where(x => x.ApplicantId == userId);
            }

            else if (companyId.HasValue)
            {
                query = query.Where(x => x.Job!.CompanyId == companyId);
            }

            query = descending
                ? query.OrderByDescending(keySelector)
                : query.OrderBy(keySelector);

            return await query.ToListAsync();
        }
   
    }
}
