using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubCommunicationService.Services.Interfaces
{
    public interface IGitHubService
    {
        Task<IEnumerable<object>> GetUserRepositoriesAsync(string token, string username, CancellationToken ct = default);
    }
}