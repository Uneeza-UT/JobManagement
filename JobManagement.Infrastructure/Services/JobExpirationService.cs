using JobManagement.Application.Contracts.Persistence;
using JobManagement.Application.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JobManagement.Infrastructure.Services
{
    public class JobExpirationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IAppLogger<JobExpirationService> _logger;

        public JobExpirationService(IServiceScopeFactory scopeFactory, IAppLogger<JobExpirationService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }


        //Checks the application deadline of a job every hour and mark its status as 'Expxired'
        // if the deadline has passed
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var jobRepository = scope.ServiceProvider
                        .GetRequiredService<IJobRepository>();

                    await jobRepository.ExpireJobsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occurred while expiring jobs.");
                }

                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }
    }
}
