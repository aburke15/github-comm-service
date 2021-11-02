using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using GitHubApiClient.Abstractions;
using GitHubCommunicationService.Abstractions;
using GitHubCommunicationService.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDatabaseAdapter.Abstractions;

namespace GitHubCommunicationService.Workers
{
    public class GitHubRepositoryBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<GitHubRepositoryBackgroundService> _logger;
        private readonly GitHubOptions _gitHubOptions;

        public GitHubRepositoryBackgroundService(
            IServiceProvider services, 
            ILogger<GitHubRepositoryBackgroundService> logger,
            IOptions<GitHubOptions> gitHubOptions)
        {
            _services = Guard.Against.Null(services, nameof(services));
            _logger = Guard.Against.Null(logger, nameof(logger));
            _gitHubOptions = Guard.Against.Null(gitHubOptions.Value, nameof(gitHubOptions.Value));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();

                    var gitHubApiService = scope.ServiceProvider
                        .GetRequiredService<IGitHubApiService>();

                    var repositories = await gitHubApiService
                        .GetUserRepositoriesFromApiAsync(stoppingToken);
                    // Persist above type after the model type is created
                    var dbRepository = scope.ServiceProvider
                        .GetRequiredService<IMongoDbRepository>();

                    Console.WriteLine($"Doing work: {count} at - [{DateTime.Now}]");
                    count++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        $"Error encountered on server. Background Service {nameof(GitHubRepositoryBackgroundService)} - {ex.Message}"
                    );
                }

                // TODO: change to TimeSpan.From(time in options file) 
                await Task.Delay(5000, stoppingToken);
            }

            _logger.LogInformation(
                $"Background Service {nameof(GitHubRepositoryBackgroundService)} has stopped at : [{DateTime.Now}]"
            );
        }
    }
}