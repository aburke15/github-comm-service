using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GitHubCommunicationService.Responses;

namespace GitHubCommunicationService.Services.Interfaces
{
    public interface IGitHubService
    {
        Task<IEnumerable<GitHubUserRepositoryResponse>> GetUserRepositoriesAsync(string token, string username, CancellationToken ct = default);
    }
}