using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using GitHubApiClient.Abstractions;
using GitHubApiClient.Models;
using GitHubCommunicationService.Abstractions;
using GitHubCommunicationService.Config;
using GitHubCommunicationService.Data.Models;
using GitHubCommunicationService.Responses;
using Microsoft.Extensions.Options;
using MongoDatabaseAdapter.Abstractions;
using MongoDatabaseAdapter.Settings;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace GitHubCommunicationService.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubOptions _gitHubOptions;
        private readonly IGitHubApiService _gitHubApiService;
        private readonly IMongoDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public GitHubService(
            IOptions<GitHubOptions> gitHubOptions,
            IGitHubApiService gitHubApiService,
            IMongoDbRepository dbRepository,
            IMapper mapper)
        {
            _gitHubOptions = Guard.Against.Null(gitHubOptions.Value, nameof(gitHubOptions));
            _gitHubApiService = Guard.Against.Null(gitHubApiService, nameof(gitHubApiService));
            _dbRepository = Guard.Against.Null(dbRepository, nameof(dbRepository));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public async Task<IEnumerable<GitHubUserRepositoryResponse>> GetUserRepositoriesAsync(CancellationToken ct = default)
        {
            var result = await _gitHubApiService.GetRepositoriesForUserAsync(ct);

            if (!result.IsSuccessful)
                throw new Exception($"An error occured: {result.Message}");

            Guard.Against.Null(result.Result, nameof(result.Result));
            var repos = result.Result;

            return _mapper.Map<IEnumerable<Repository>, IEnumerable<GitHubUserRepositoryResponse>>(repos);
        }

        public async Task<IEnumerable<Reservation>> GetAllUserRepositoriesFromDbAsync(
            string databaseName, string collectionName, CancellationToken ct = default)
        {
            var connectionSettings = new MongoDbConnectionSettings()
            {
                DatabaseName = databaseName,
                CollectionName = collectionName
            };
            
            return await _dbRepository.GetAllAsync<Reservation>(connectionSettings, ct);
        }
    }
}