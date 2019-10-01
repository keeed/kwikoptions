using System;
using System.Reflection;
using KwikOptions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KwikOptions
{
    public class DynamicKwikOptionsService : IKwikOptionsService
    {
        public DynamicKwikOptionsService(
            IServiceCollection services,
            IConfiguration configuration,
            RootKwikOptions rootKwikOptions)
        {
            Services = services;
            Configuration = configuration;
            RootKwikOptions = rootKwikOptions;
        }

        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
        public RootKwikOptions RootKwikOptions { get; }

        public void InjectOptions()
        {
            foreach (var optionsType in RootKwikOptions.OptionsTypes)
            {
                if (RootKwikOptions.LoadExternalAssemblies)
                {
                    AssemblyUtilities.LoadAssemblyIfNotLoaded(optionsType.Assembly);
                }

                try
                {
                    MethodInfo method =
                        typeof(OptionsConfigurationServiceCollectionExtensions).GetMethod(
                            "Configure",
                            BindingFlags.Static | BindingFlags.Public,
                            null,
                            new[] { typeof(IServiceCollection), typeof(IConfigurationSection) },
                            null);
                    MethodInfo generic = method.MakeGenericMethod(Type.GetType(optionsType.Type));
                    generic.Invoke(null, new object[] { Services, Configuration.GetSection(optionsType.OptionsPath) });
                }
                catch (Exception)
                {

                }
            }
        }
    }
}