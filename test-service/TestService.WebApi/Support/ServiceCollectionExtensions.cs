using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TestService.WebApi.Support
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddSettings<TSettings>(this IServiceCollection services, IConfigurationSection section) where TSettings : class, new()
        {
            services.Configure<TSettings>(section);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<TSettings>>().Value);
        }
    }
}
