using ABU.GitHubCommunicationService.Core.Data.Models;
using ABU.GitHubCommunicationService.Infrastructure.Abstractions;
using Ardalis.GuardClauses;
using AutoMapper;
using GitHubApiClient.Abstractions;
using Newtonsoft.Json;

namespace ABU.GitHubCommunicationService.Infrastructure.Services;

public class GitHubApiService : IGitHubApiService
{
    private readonly IGitHubApiClient _gitHubApiClient;
    private readonly IMapper _mapper;

    public GitHubApiService(
        IGitHubApiClient gitHubApiClient,
        IMapper mapper)
    {
        _gitHubApiClient = Guard.Against.Null(gitHubApiClient, nameof(gitHubApiClient));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
    }

    public async Task<IReadOnlyList<Repository>> GetUserRepositoriesFromApiAsync(CancellationToken ct = default)
    {
        var result = await _gitHubApiClient.GetRepositoriesForUserAsync(ct);

        if (!result.IsSuccessful)
            throw new Exception($"An error occured: {result.Message}");

        result = Guard.Against.Null(result, nameof(result));
        var json = result.Json;

        if (string.IsNullOrWhiteSpace(json))
            return new List<Repository>();

        return JsonConvert.DeserializeObject<IReadOnlyList<Repository>>(json)!;
    }
}