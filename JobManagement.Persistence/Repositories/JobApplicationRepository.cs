using JobManagement.Application.Contracts.Persistence;
using JobManagement.Application.DTOs.Common;
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
        public async Task<List<JobApplication>> GetByUserIdAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            int skip = (pageNumber - 1) * pageSize;

            return await _dbContext.Set<JobApplication>()
                .AsNoTracking()
                .Where(j =>
                    j.UserId == userId)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }



        // Retrieves job applications submitted for a particular job.
        public async Task<List<JobApplication>> GetByJobIdAsync(int jobId, int pageNumber = 1, int pageSize = 10)
        {
            int skip = (pageNumber - 1) * pageSize;

            return await _dbContext.Set<JobApplication>()
                .AsNoTracking()
                .Where(j =>
                    j.JobId == jobId)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
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
