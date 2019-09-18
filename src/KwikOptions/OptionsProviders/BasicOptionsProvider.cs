using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KwikOptions.OptionsProviders
{
    public abstract class BasicOptionsProvider<T> : IOptionsProvider 
        where T : class
    {
        public void ConfigureOption(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<T>(configuration);
        }
    }
}