using JobManagement.Application.Contracts.Persistence;
using JobManagement.Application.DTOs.Job;
using JobManagement.Domain;
using JobManagement.Domain.Enums;
using JobManagement.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace JobManagement.Persistence.Repositories
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        public JobRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }


        // Retrieves job entities specific to a particular company.
        public async Task<IReadOnlyList<Job>> GetByCompanyIdAsync(int companyId, int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;

            return await _dbContext.Set<Job>()
                .AsNoTracking()
                .Where(j => j.CompanyId == companyId)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }



        // Retrieves a specific job entity including company entity.
        public async Task<Job?> GetByIdWithCompanyAsync(int id)
        {
            return await _dbContext.Set<Job>()
                .AsNoTracking()
                .Include(j => j.Company)
                .FirstOrDefaultAsync(j => j.Id == id);
        }



        // Retrieves job entities that match the search term across the specified string properties.
        public async Task<IReadOnlyList<Job>> SearchAsync(string searchTerm)
        {
            return await _dbContext.Set<Job>()
                .AsNoTracking()
                .Where(j =>
                    j.Title.Contains(searchTerm) ||
                    j.Location.Contains(searchTerm) ||
                    j.Company!.Name.Contains(searchTerm) ||
                    j.JobType.ToString().Contains(searchTerm) ||
                    j.EmploymentType.ToString().Contains(searchTerm))
                .ToListAsync();
        }


        //Counts the active jobs with the same entity properties.
        public async Task<int> CountActiveJobsWithSameTitleAsync(string title, int companyId, int? excludeJobId = null)
        {
            return await _dbContext.Set<Job>()
                .AsNoTracking()
                .CountAsync(j =>
                    j.CompanyId == companyId &&
                    j.Id != excludeJobId &&
                    j.Title == title &&
                    (j.ApprovalStatus == JobApprovalStatus.Pending ||
                    j.ApprovalStatus == JobApprovalStatus.Accepted));
        }


        //Changes the status of jobs whose deadline has passed to Expired
        public async Task ExpireJobsAsync()
        {
            var jobs = await _dbContext.Jobs
                .Where(x =>
                    x.ApplicationDeadline <= DateTime.UtcNow &&
                    x.ApprovalStatus == JobApprovalStatus.Pending)
                .ToListAsync();

            foreach (var job in jobs)
            {
                job.ApprovalStatus = JobApprovalStatus.Expired;
            }

            await _dbContext.SaveChangesAsync();
        }
    
    }
}
