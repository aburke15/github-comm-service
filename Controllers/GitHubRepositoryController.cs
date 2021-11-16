using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using GitHubCommunicationService.Abstractions;
using GitHubCommunicationService.Data.Models;
using GitHubCommunicationService.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDatabaseAdapter.Abstractions;
using MongoDatabaseAdapter.Settings;
using MongoDB.Driver;

namespace GitHubCommunicationService.Controllers;

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
            Console.WriteLine($"Executing method: {nameof(FindFromRepoAsync)} at: {DateTime.Now}");

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
            Console.WriteLine($"Executing method: {nameof(FindAllFromRepoAsync)} at: {DateTime.Now}");

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

    [HttpGet]
    public async Task<IActionResult> GetFromTwoDbsAsync()
    {
        try
        {
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e);
        }
    }
}