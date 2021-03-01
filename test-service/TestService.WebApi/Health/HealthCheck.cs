using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TestService.WebApi.Health
{
   [ExcludeFromCodeCoverage]
   public class HealthCheck : IHealthCheck
   {
      public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
         CancellationToken cancellationToken = new CancellationToken())
      {
         // Execute additional health check logic here.
         return Task.FromResult(HealthCheckResult.Healthy());
      }
   }
}
