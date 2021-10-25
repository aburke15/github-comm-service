using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using GitHubCommunicationService.Abstractions;
using GitHubCommunicationService.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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

                    var gitHubService = scope.ServiceProvider
                        .GetRequiredService<IGitHubService>();

                    // var results = await gitHubService.GetUserRepositoriesAsync(
                    //     token: _gitHubOptions.AuthToken, 
                    //     username: _gitHubOptions.Username, 
                    //     ct: stoppingToken);
                    
                    // Just a to test the library
                    // var items = await gitHubService
                    //     .GetAllUserRepositoriesFromDbAsync("hello", "Reservations");
                    //
                    // // TODO: persist to database
                    // Console.WriteLine(items);
                    // Console.WriteLine(results);

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