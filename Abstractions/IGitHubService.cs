using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GitHubCommunicationService.Data.Models;
using GitHubCommunicationService.Responses;

namespace GitHubCommunicationService.Abstractions
{
    public interface IGitHubService
    {
        Task<IEnumerable<GitHubUserRepositoryResponse>> GetUserRepositoriesAsync(string token, string username, CancellationToken ct = default);

        Task<IEnumerable<Reservation>> GetAllUserRepositoriesFromDbAsync(string databaseName,
            string collectionName);
    }
}