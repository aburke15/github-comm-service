using System;
using System.Threading;
using System.Threading.Tasks;
using GitHubCommunicationService.Abstractions;
using GitHubCommunicationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitHubCommunicationService.Controllers
{
    [ApiController]
    [Route("api/repos")]
    public class GitHubRepositoryController : ControllerBase
    {
        private readonly IGitHubApiService _gitHubApiService;
        
        public GitHubRepositoryController(IGitHubApiService gitHubApiService)
        {
            _gitHubApiService = gitHubApiService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(5000);
            return Ok();
        }

        [HttpGet("tests")]
        public async Task<IActionResult> GetTestData(CancellationToken ct)
        {
            await Task.Delay(1000, ct);
            throw new NotImplementedException();
        }

        [HttpGet("repos")]
        public async Task<IActionResult> GetRepoData(CancellationToken ct)
        {
            return Ok(await _gitHubApiService.GetUserRepositoriesFromApiAsync(ct));
        }
    }
}