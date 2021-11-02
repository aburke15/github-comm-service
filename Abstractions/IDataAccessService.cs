using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GitHubCommunicationService.Abstractions
{
    public interface IDataAccessService
    {
        // TODO: figure out methods should go here
        Task CreateDocumentsAsync<T>(string databaseName, string collectionName, IEnumerable<T> documents, CancellationToken ct = default);
    }
}