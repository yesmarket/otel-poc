using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TestService.WebApi.Controllers
{
   [ApiVersion("1.0")]
   [ApiController]
   [Route("v{version:apiVersion}/test")]
   public class TestController : ControllerBase
   {
      private readonly ILogger<TestController> _logger;

      public TestController(
         ILogger<TestController> logger)
      {
          _logger = logger;
      }

      [HttpGet]
      public async Task<IActionResult> GetAsync()
      {
          _logger.LogInformation($"Called {nameof(GetAsync)}");

          return Ok();
      }

        [HttpPost]
      public async Task<IActionResult> PostAsync()
      {
         _logger.LogInformation($"Called {nameof(PostAsync)}");

         return Ok();
      }
   }
}
