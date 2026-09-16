using JobManagement.Application.Contracts.Persistence;
using JobManagement.Domain;
using JobManagement.Persistence.DatabaseContext;

namespace JobManagement.Persistence.Repositories
{
    public class JobApplicationRepository : GenericRepository<JobApplication>, IJobApplicationRepository
    {
        public JobApplicationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
