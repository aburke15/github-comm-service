using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using GitHubCommunicationService.Config;
using GitHubCommunicationService.Responses;
using GitHubCommunicationService.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace GitHubCommunicationService.Services.Implementations
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubOptions _gitHubOptions;
        private readonly IRestClient _client;

        public GitHubService(
            IOptions<GitHubOptions> gitHubOptions,
            IRestClient client)
        {
            _gitHubOptions = Guard.Against.Null(gitHubOptions.Value, nameof(gitHubOptions));
            _client = Guard.Against.Null(client, nameof(client));
        }

        public async Task<IEnumerable<GitHubUserRepositoryResponse>> GetUserRepositoriesAsync(
            string token, string username, CancellationToken ct = default)
        {
            token = Guard.Against.NullOrWhiteSpace(token, nameof(token));
            username = Guard.Against.NullOrWhiteSpace(username, nameof(username));

            _client.BaseUrl = new Uri(_gitHubOptions.BaseUri);
            _client.Authenticator = new JwtAuthenticator(token);

            var request = new RestRequest(
                $"/users/{username}/repos", Method.GET, DataFormat.Json
            ) as IRestRequest;

            var response = await _client.ExecuteGetAsync(request, ct);

            if (!response.IsSuccessful)
                return Enumerable.Empty<GitHubUserRepositoryResponse>();

            return JsonConvert.DeserializeObject<IEnumerable<GitHubUserRepositoryResponse>>(response.Content);
        }
    }
}