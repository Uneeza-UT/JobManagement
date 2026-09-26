using JobManagement.Application.Contracts.Persistence;
using JobManagement.Domain;
using JobManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

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



        // Retrieves job application entities that match the search term across the specified string properties.
        public async Task<IReadOnlyList<JobApplication>> SearchAsync(string searchTerm)
        {
            return await _dbContext.Set<JobApplication>()
                .AsNoTracking()
                .Where(j =>
                    j.FirstName.Contains(searchTerm) ||
                    j.LastName.Contains(searchTerm) ||
                    j.Email.Contains(searchTerm) ||
                    j.PhoneNumber.Contains(searchTerm) ||
                    j.Job!.Title.Contains(searchTerm))
                .ToListAsync();
        }


        //Ensure that no other applicant has applied with the same email address for a job
        public async Task<bool> IsEmailUniqueAsync(string email, int jobId)
        {
            return !await _dbContext.JobApplications
                .AnyAsync(j => 
                    j.JobId == jobId &&
                    j.Email == email);
        }
    }
}
