using JobManagement.Application.Contracts.Email;
using JobManagement.Application.Contracts.Services;
using JobManagement.Application.Logging;
using JobManagement.Infrastructure.EmailService;
using JobManagement.Infrastructure.Logging;
using JobManagement.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;


namespace JobManagement.Infrastructure;

public static class InfrastructureServiceRegistration
{
    // Registers application infrastructure services such as
    // email, and external service integrations.
    
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, 
        IConfiguration configuration)
    {
        
        services.AddOptions();
        services.AddHttpClient<ResendClient>();
        services.Configure<ResendClientOptions>(options =>
        {
            options.ApiToken = configuration.GetValue<string>("Resend:ApiKey");
        });

        services.AddTransient<IResend, ResendClient>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddScoped<IGenericService, GenericService>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IJobApplicationService, JobApplicationService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<CurrentUserService>();
        services.AddHostedService<JobExpirationService>();
        services.AddHttpClient<IFileStorageService, FileStorageService>();

        return services;
    }
}
