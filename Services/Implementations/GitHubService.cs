using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using GitHubCommunicationService.Config;
using GitHubCommunicationService.Data.Models;
using GitHubCommunicationService.Responses;
using GitHubCommunicationService.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDatabaseAdapter.Abstractions;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace GitHubCommunicationService.Services.Implementations
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubOptions _gitHubOptions;
        private readonly IRestClient _client;
        private readonly IMongoDbRepository _dbRepository;

        public GitHubService(
            IOptions<GitHubOptions> gitHubOptions,
            IRestClient client,
            IMongoDbRepository dbRepository)
        {
            _gitHubOptions = Guard.Against.Null(gitHubOptions.Value, nameof(gitHubOptions));
            _client = Guard.Against.Null(client, nameof(client));
            _dbRepository = Guard.Against.Null(dbRepository, nameof(dbRepository));
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

        public Task<IEnumerable<Reservation>> GetAllUserRepositoriesFromDbAsync(string dbName, string collectionName)
        {
            return _dbRepository.GetAllAsync<Reservation>(dbName, collectionName);
        }
    }
}