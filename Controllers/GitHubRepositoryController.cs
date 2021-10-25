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
        private readonly IGitHubService _gitHubService;
        
        public GitHubRepositoryController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
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
            var (databaseName, collectionName) = ("restaurantHoster", "Reservations");
            return Ok(await _gitHubService.GetAllUserRepositoriesFromDbAsync(databaseName, collectionName, ct));
        }
    }
}