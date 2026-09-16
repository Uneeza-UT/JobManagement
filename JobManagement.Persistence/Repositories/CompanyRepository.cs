using JobManagement.Application.Contracts.Persistence;
using JobManagement.Domain;
using JobManagement.Persistence.DatabaseContext;

namespace JobManagement.Persistence.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
