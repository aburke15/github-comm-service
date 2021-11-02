using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using GitHubCommunicationService.Abstractions;
using MongoDatabaseAdapter.Abstractions;
using MongoDatabaseAdapter.Repositories;
using MongoDatabaseAdapter.Settings;

namespace GitHubCommunicationService.Services
{
    public class DataAccessService : IDataAccessService
    {
        // TODO: once the methods are decided in the interface fill out the implementation details
        private readonly IMongoDbRepository _dbRepository;
        
        public DataAccessService(IMongoDbRepository dbRepository)
        {
            _dbRepository = Guard.Against.Null(dbRepository, nameof(dbRepository));
        }
        
        public async Task CreateDocumentsAsync<T>(string databaseName, string collectionName, IEnumerable<T> documents, CancellationToken ct = default)
        {
            var settings = new MongoDbConnectionSettings
            {
                DatabaseName = databaseName,
                CollectionName = collectionName
            };

            // TODO: set the correct document type
            await _dbRepository.InsertManyAsync<object>(settings, documents as IEnumerable<object>, ct);
        }
    }
}