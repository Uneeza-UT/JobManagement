using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JobManagement.Persistence.DatabaseContext;
using JobManagement.Application.Contracts.Persistence;
using JobManagement.Persistence.Repositories;
using JobManagement.Application.Contracts.Services;

namespace JobManagement.Persistence;

public static class PersistenceServiceRegistration
{
    // Registers dependencies required by the Persistence layer,
    // including the database context and repositories.

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("JobManagementDbConnectionString"));
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyJoinRequestRepository, CompanyJoinRequestRepository>();

        return services;
    }
}
