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
      public IActionResult Get()
      {
          _logger.LogInformation($"Called {nameof(Get)}");

          return Ok();
      }

      [HttpPost]
      public IActionResult Post()
      {
         _logger.LogInformation($"Called {nameof(Post)}");

         return Ok();
      }
   }
}
