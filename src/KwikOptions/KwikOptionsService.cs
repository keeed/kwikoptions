using System;
using System.Linq;
using System.Reflection;
using KwikOptions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KwikOptions
{
    public class KwikOptionsService
    {
        public KwikOptionsService(
            IServiceCollection serviceCollection,
            IConfiguration configuration)
            : this("KwikOptions", serviceCollection, configuration)
        {

        }

        public KwikOptionsService(
            string configPath,
            IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            ConfigPath = configPath;
            ServiceCollection = serviceCollection;
            Configuration = configuration;
        }

        public string ConfigPath { get; }
        public IServiceCollection ServiceCollection { get; }
        public IConfiguration Configuration { get; }

        public void InjectOptions()
        {
            var rootKwikOptions = new RootKwikOptions();
            Configuration.GetSection(ConfigPath).Bind(rootKwikOptions);

            foreach (var optionsProvider in rootKwikOptions.OptionsProviders)
            {
                LoadAssemblyIfNotLoaded(optionsProvider.Assembly);
                Type optionsType = Type.GetType(optionsProvider.Type);
                var providerInstance =
                    (IOptionsProvider)ActivatorUtilities
                        .CreateInstance(ServiceCollection.BuildServiceProvider(), optionsType);

                providerInstance
                    .ConfigureOption(
                        ServiceCollection, 
                        Configuration.GetSection(optionsProvider.OptionsPath));
            }
        }

        private void LoadAssemblyIfNotLoaded(string assemblyName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var assembly = assemblies.FirstOrDefault(
                    a => a.GetName().Name == assemblyName.Remove(assemblyName.Length - 4));

            if (assembly == null)
            {
                Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + assemblyName);
            }
        }
    }
}
