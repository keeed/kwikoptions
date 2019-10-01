using KwikOptions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KwikOptions
{
    public static class KwikOptionsServiceCollectionExtensions
    {
        public static IServiceCollection UswKwikOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return UseKwikOptions(services, configuration, "KwikOptions");
        }

        public static IServiceCollection UseKwikOptions(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationRootPath)
        {
            var rootKwikOptions = new RootKwikOptions();
            configuration.GetSection(configurationRootPath).Bind(rootKwikOptions);

            var kwikOptionsService = new DynamicKwikOptionsService(
                services,
                configuration,
                rootKwikOptions
            );

            kwikOptionsService.InjectOptions();

            return services;
        }
    }
}