using GitHubCommunicationService.Services.Interfaces;

namespace GitHubCommunicationService.Services.Implementations
{
    public class GitHubService : IGitHubService
    {
        private readonly IGitHubHttpClient _gitHubHttpClient;
        
        public GitHubService(IGitHubHttpClient gitHubHttpClient)
        {
            _gitHubHttpClient = gitHubHttpClient;
        }
    }
}