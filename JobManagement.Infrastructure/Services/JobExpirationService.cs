using JobManagement.Application.Contracts.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobManagement.Infrastructure.Services
{
    public class JobExpirationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public JobExpirationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        //Checks the application deadline of a job every hour and mark its status as 'Expxired'
        // if the deadline has passed
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var jobRepository = scope.ServiceProvider
                    .GetRequiredService<IJobRepository>();

                await jobRepository.ExpireJobsAsync();

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
