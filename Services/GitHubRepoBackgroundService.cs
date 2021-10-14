using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GitHubCommunicationService.Services
{
    public class GitHubRepoBackgroundService : BackgroundService
    {
        private readonly ILogger<GitHubRepoBackgroundService> _logger;

        public GitHubRepoBackgroundService(ILogger<GitHubRepoBackgroundService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine($"Doing work: {count} at - [{DateTime.Now}]");
                    count++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        $"Error encountered on server. Background Service {nameof(GitHubRepoBackgroundService)} : {0}", ex.Message
                    );
                }

                await Task.Delay(1000, stoppingToken);
            }

            _logger.LogInformation(
                $"Background Service {nameof(GitHubRepoBackgroundService)} has stopped at : [{DateTime.Now}]"
            );
        }
    }
}