using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KwikOptions
{
    public interface IOptionsProvider 
    {
        void ConfigureOption(IServiceCollection serviceCollection, IConfiguration configuration);
    }
}