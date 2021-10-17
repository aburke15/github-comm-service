using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using GitHubCommunicationService.Config;
using GitHubCommunicationService.Services.Interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;

namespace GitHubCommunicationService.Services.Implementations
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubOptions _gitHubOptions;

        public GitHubService(IOptions<GitHubOptions> gitHubOptions)
        {
            _gitHubOptions = Guard.Against.Null(gitHubOptions.Value, nameof(gitHubOptions));
        }

        public async Task<IEnumerable<object>> GetUserRepositoriesAsync(
            string token, string username, CancellationToken ct = default)
        {
            var client = new RestClient(_gitHubOptions.BaseUri) as IRestClient;
            client.Authenticator = new JwtAuthenticator(token);

            var request = new RestRequest(
                $"/users/{username}/repos", Method.GET, DataFormat.Json
            ) as IRestRequest;

            var response = await client.ExecuteGetAsync(request, ct);

            if (!response.IsSuccessful)
                Console.WriteLine("Failed request ", response.ErrorMessage);

            Console.WriteLine(response?.Content);

            return new List<object>();
        }
    }
}