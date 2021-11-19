using ABU.GitHubCommunicationService.Core.Config;
using ABU.GitHubCommunicationService.Core.Data.Models;
using ABU.GitHubCommunicationService.Infrastructure.Abstractions;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using MongoDatabaseAdapter.Abstractions;
using MongoDatabaseAdapter.Settings;

namespace ABU.GitHubCommunicationService.Infrastructure.Workers;

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
                // TODO: refactor this 
                using var scope = _services.CreateScope();
                var newRepos = new List<Repository>() as IList<Repository>;

                var gitHubApiService = scope.ServiceProvider
                    .GetRequiredService<IGitHubApiService>();
                var dbRepository = scope.ServiceProvider
                    .GetRequiredService<IMongoDbRepository>();

                var githubRepos = await gitHubApiService
                    .GetUserRepositoriesFromApiAsync(stoppingToken);

                var settings = new MongoDbConnectionSettings
                {
                    DatabaseName = "github",
                    CollectionName = "repositories"
                };

                var dbRepos = await dbRepository
                    .GetAllAsync<Repository>(settings, stoppingToken);

                if (dbRepos.Count == 0)
                {
                    Console.WriteLine("Inserting github repos into db.");
                    await dbRepository.InsertManyAsync(settings, githubRepos, stoppingToken);
                    Console.WriteLine($"Inserted {githubRepos.Count} repos into db.");
                    continue;
                }

                if (dbRepos.Count > 0)
                {
                    var repoIds = dbRepos.Select(x => x.Id).ToList();
                    foreach (var repo in githubRepos)
                    {
                        if (!repoIds.Contains(repo.Id))
                            newRepos.Add(repo);
                    }

                    if (newRepos.Count > 0)
                    {
                        Console.WriteLine($"Inserting new repos into db.");
                        await dbRepository.InsertManyAsync(settings, newRepos, stoppingToken);
                        Console.WriteLine($"Inserted {newRepos.Count} repos into db.");
                    }
                }

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
            await Task.Delay(10000, stoppingToken);
        }

        _logger.LogInformation(
            $"Background Service {nameof(GitHubRepositoryBackgroundService)} has stopped at : [{DateTime.Now}]"
        );
    }
}