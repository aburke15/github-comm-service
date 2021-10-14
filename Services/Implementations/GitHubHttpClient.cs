using System.Net.Http;
using GitHubCommunicationService.Services.Interfaces;

namespace GitHubCommunicationService.Services.Implementations
{   
    public class GitHubHttpClient : IGitHubHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GitHubHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }   
}