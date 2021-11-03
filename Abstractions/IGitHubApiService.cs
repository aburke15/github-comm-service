using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GitHubCommunicationService.Data.Models;
using GitHubCommunicationService.Responses;

namespace GitHubCommunicationService.Abstractions
{
    public interface IGitHubApiService
    {
        Task<IEnumerable<Repository>> GetUserRepositoriesFromApiAsync(CancellationToken ct = default);
    }
}