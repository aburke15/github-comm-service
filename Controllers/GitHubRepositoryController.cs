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

            var result = await _dbRepository.FindOneAsync(_settings, Builders<Repository>.Filter.Eq("Name", "blog-api"), ct);

            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e);
        }
    }
}