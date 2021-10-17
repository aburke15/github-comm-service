using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using GitHubCommunicationService.Config;
using GitHubCommunicationService.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GitHubCommunicationService.Services.Implementations
{
    public class GitHubRepoBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<GitHubRepoBackgroundService> _logger;
        private readonly GitHubOptions _gitHubOptions;

        public GitHubRepoBackgroundService(
            IServiceProvider services, 
            ILogger<GitHubRepoBackgroundService> logger,
            IOptions<GitHubOptions> gitHubOptions)
        {
            _services = Guard.Against.Null(services, nameof(services));
            _logger = Guard.Against.Null(logger, nameof(logger));
            _gitHubOptions = Guard.Against.Null(gitHubOptions.Value, nameof(gitHubOptions));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();

                    var _gitHubService = scope.ServiceProvider
                        .GetRequiredService<IGitHubService>();

                    var resutls = await _gitHubService.GetUserRepositoriesAsync(
                        token: _gitHubOptions.AuthToken, 
                        username: _gitHubOptions.Username, 
                        ct: stoppingToken);

                    Console.WriteLine(resutls);

                    Console.WriteLine($"Doing work: {count} at - [{DateTime.Now}]");
                    count++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        $"Error encountered on server. Background Service {nameof(GitHubRepoBackgroundService)} - {ex.Message}"
                    );
                }

                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogInformation(
                $"Background Service {nameof(GitHubRepoBackgroundService)} has stopped at : [{DateTime.Now}]"
            );
        }
    }
}