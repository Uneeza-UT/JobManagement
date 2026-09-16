using JobManagement.Application.Contracts.Email;
using JobManagement.Application.Logging;
using JobManagement.Infrastructure.EmailService;
using JobManagement.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Resend;


namespace JobManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
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

            return services;
        }
    }
}
