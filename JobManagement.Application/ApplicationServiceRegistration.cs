using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JobManagement.Application;

public static class ApplicationServiceRegistration
{
    // Registers services and dependencies belonging to the Application layer.
    
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
