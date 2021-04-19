using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
//using OpenTelemetry;
//using OpenTelemetry.Resources;
//using OpenTelemetry.Trace;
using Serilog;
using TestService.WebApi.Support;

namespace TestService.WebApi
{
   [ExcludeFromCodeCoverage]
   public static class Program
   {
      public static async Task Main(string[] args)
      {
         await CreateHostBuilder(args).RunConsoleAsync().ConfigureAwait(false);
      }

      public static IHostBuilder CreateHostBuilder(string[] args)
      {
         return
            Host.CreateDefaultBuilder(args)
               .UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration))
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureWebHostDefaults(webBuilder =>
               {
                  webBuilder
                     .ConfigureAppConfiguration((context, builder) =>
                     {
                        var basePath = Environment.GetEnvironmentVariable("CONFIG_DIR") ?? Directory.GetCurrentDirectory();
                        var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
                        builder.SetBasePath(basePath);
                        builder.AddJsonFile("appsettings.json", false, true);
                        builder.AddEnvironmentVariables();
                     })
                     .UseKestrel(o =>
                     {
                        o.ListenAnyIP(Environment.GetEnvironmentVariable("PORT_NUMBER").AsPortNumberOrDefaultTo(5001),
                           lo =>
                           {
                              lo.Protocols = HttpProtocols.Http1;
                           });
                     })
                     .UseStartup<Startup>();
               });
      }
   }
}
