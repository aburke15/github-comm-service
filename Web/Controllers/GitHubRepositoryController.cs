using System.Reflection;
using ABU.GitHubCommunicationService.Core.Data.Models;
using ABU.GitHubCommunicationService.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MongoDatabaseAdapter.Abstractions;
using MongoDatabaseAdapter.Settings;
using MongoDB.Driver;

namespace ABU.GitHubCommunicationService.Web.Controllers;

[ApiController]
[Route("api/repos")]
public class GitHubRepositoryController : ControllerBase
{
    private readonly IGitHubApiService _gitHubApiService;
    private readonly IMongoDbRepository _dbRepository;

    private readonly MongoDbConnectionSettings _settings;

    public GitHubRepositoryController(
        IGitHubApiService gitHubApiService,
        IMongoDbRepository dbRepository)
    {
        _gitHubApiService = gitHubApiService;
        _dbRepository = dbRepository;
        _settings = new MongoDbConnectionSettings()
        {
            DatabaseName = "github",
            CollectionName = "repositories"
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetReposFromApi(CancellationToken ct)
    {
        return Ok(await _gitHubApiService.GetUserRepositoriesFromApiAsync(ct));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRepoFromDbById(string id, CancellationToken ct)
    {
        try
        {
            Console.WriteLine($"objectId: {id}");

            var result = await _dbRepository
                .GetByIdAsync<Repository>(_settings, id, ct);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e);
        }
    }

    [HttpGet("find/{id}")]
    public async Task<IActionResult> FindFromRepoAsync(string id, CancellationToken ct)
    {
        try
        {
            Console.WriteLine($"Executing method: {MethodBase.GetCurrentMethod()?.Name} at: {DateTime.Now}");

            var filter = Builders<Repository>.Filter.Eq(fp => fp.Name, "blog-api");

            var result = await _dbRepository.FindOneAsync(_settings, filter, ct);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e);
        }
    }

    [HttpGet("find-all")]
    public async Task<IActionResult> FindAllFromRepoAsync(CancellationToken ct)
    {
        try
        {
            Console.WriteLine($"Executing method: {MethodBase.GetCurrentMethod()?.Name} at: {DateTime.Now}");

            var filter = Builders<Repository>.Filter.Lt(fp => fp.Size, 100);

            var results = await _dbRepository.FindManyAsync(_settings, filter, ct);

            return Ok(results);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e);
        }
    }

    [HttpGet("get-two")]
    public async Task<IActionResult> GetFromTwoDbsAsync(CancellationToken ct)
    {
        try
        {
            Console.WriteLine($"Executing method: {MethodBase.GetCurrentMethod()?.Name} at: {DateTime.Now}");

            var filter = Builders<Repository>.Filter.Lt(fp => fp.Size, 100);

            var results1 = await _dbRepository.FindManyAsync(_settings, filter, ct);
            var results2 = await _dbRepository.GetAllAsync<Test>(new MongoDbConnectionSettings { DatabaseName = "test", CollectionName = "tests"}, ct);
            
            foreach (var res in results2)
                Console.WriteLine(res.ToString());

            return Ok(results1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e);
        }
    }
}