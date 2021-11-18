using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABU.GitHubCommunicationService.Data.Models;
using ABU.GitHubCommunicationService.Responses;

namespace ABU.GitHubCommunicationService.Abstractions
{
    public interface IGitHubApiService
    {
        Task<IReadOnlyList<Repository>> GetUserRepositoriesFromApiAsync(CancellationToken ct = default);
    }
}