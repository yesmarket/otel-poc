using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using Flurl;
using Galaxy.Common.Configuration;
using Galaxy.Common.ErrorHandling;
using Galaxy.Common.Security.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog.Enrichers.Correlation;
using TestService.WebApi.Health;
using TestService.WebApi.Support;

namespace TestService.WebApi
{
   [ExcludeFromCodeCoverage]
   public class Startup
   {
      private readonly IConfiguration _configuration;

      public Startup(IConfiguration configuration)
      {
         _configuration = configuration;
      }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddSettings<ApplicationSettings>(_configuration.GetSection("applicationSettings"));

         services.AddApiVersioning(options => options.ReportApiVersions = true);

         services
            .AddMvc()
            .AddNewtonsoftJson()
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
         var serviceProvider = services.BuildServiceProvider();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
         var appSettings = serviceProvider.GetService<ApplicationSettings>();

         if (appSettings.SwaggerUIEnabled)
         {
            services.AddSwaggerGen(c =>
            {
               c.SwaggerDoc(appSettings.ServiceName,
                  new OpenApiInfo
                  {
                     Title = appSettings.ServiceName,
                     Version = Assembly.GetEntryAssembly()?
                        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                        .InformationalVersion ?? "NA"
                  });
            });
         }

         services.AddCors(o => o.AddPolicy("AllowAnyOrigin", builder =>
         {
            builder
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
         }));

         //if (appSettings.AuthenticationEnabled)
         //{
         //   services.AddAuthN();
         //   services.AddAuthZ();
         //}

         if (appSettings.HealthChecksEnabled)
         {
            services.AddSingleton<HealthCheck>();
            services.AddHealthChecks()
               .AddCheck<HealthCheck>(appSettings.ServiceName);
         }

         services.AddControllers();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public static void Configure(
         IApplicationBuilder app,
         IServiceProvider serviceProvider)
      {
         var logger = serviceProvider.GetService<ILogger<Startup>>();

         try
         {
            var appSettings = serviceProvider.GetService<IOptions<ApplicationSettings>>().Value;

            if (appSettings.SwaggerUIEnabled)
            {
               app.UseStaticFiles()
                  .UseSwagger()
                  .UseSwaggerUI(c =>
                  {
                     c.SwaggerEndpoint(Url.Combine("/", "swagger.json"), appSettings.ServiceName);
                  });
            }

            app.UseExceptionMiddleware(new Dictionary<HttpStatusCode, IEnumerable<Type>>
            {
               {HttpStatusCode.Unauthorized, new[] {typeof(AuthorizationException)}}
            });

            app.UseRouting();

            app.UseCors("AllowAnyOrigin");

            if (appSettings.AuthenticationEnabled)
            {
               app.UseAuthentication();
               app.UseAuthorization();
            }

            app.UseEndpoints(endpoints =>
            {
               endpoints.MapControllers();

               if (appSettings.HealthChecksEnabled)
               {
                  endpoints.MapHealthChecks(appSettings.HealthChecksPath ?? "/health");
               }

               if (appSettings.MetricsEnabled)
               {
                  endpoints.MapMetrics(appSettings.MetricsPath ?? "/metrics");
               }
            });

#pragma warning disable CA2000 // Dispose objects before losing scope
            DiagnosticListener.AllListeners.Subscribe(new CorrelationIdObserver());
#pragma warning restore CA2000 // Dispose objects before losing scope
         }
         catch (Exception ex)
         {
            var baseEx = ex.GetBaseException();
            logger.LogCritical($"Error during configuration of HTTP request pipeline:{Environment.NewLine}{baseEx.Message}");
#if DEBUG
            Console.ReadLine();
#endif
            throw;
         }
      }
   }
}
