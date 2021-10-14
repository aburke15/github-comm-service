using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GitHubCommunicationService.Controllers
{
    [ApiController]
    [Route("api/repos")]
    public class GitHubRepositoryController : ControllerBase
    {
        public GitHubRepositoryController()
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await Task.Delay(5000);
            return Ok();
        }
    }
}