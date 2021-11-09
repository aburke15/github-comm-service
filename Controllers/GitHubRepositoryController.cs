using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using GitHubCommunicationService.Abstractions;
using GitHubCommunicationService.Data.Models;
using GitHubCommunicationService.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDatabaseAdapter.Abstractions;
using MongoDatabaseAdapter.Settings;

namespace GitHubCommunicationService.Controllers
{
    [ApiController]
    [Route("api/repos")]
    public class GitHubRepositoryController : ControllerBase
    {
        private readonly IGitHubApiService _gitHubApiService;
        private readonly IMongoDbRepository _dbRepository;
        
        public GitHubRepositoryController(
            IGitHubApiService gitHubApiService,
            IMongoDbRepository dbRepository)
        {
            _gitHubApiService = gitHubApiService;
            _dbRepository = dbRepository;
        }

        [HttpGet("tests")]
        public async Task<IActionResult> GetTestData(CancellationToken ct)
        {
            await Task.Delay(1000, ct);
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetReposFromApi(CancellationToken ct)
        {
            return Ok(await _gitHubApiService.GetUserRepositoriesFromApiAsync(ct));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRepoFromDbById(string id, CancellationToken ct)
        {
            Console.WriteLine($"objectId: {id}");
            var settings = new MongoDbConnectionSettings
            {
                DatabaseName = "github",
                CollectionName = "repositories"
            };

            return Ok(await _dbRepository.GetByIdAsync<Repository>(settings, id, ct));
        }
    }
}