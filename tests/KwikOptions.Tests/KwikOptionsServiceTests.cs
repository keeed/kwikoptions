using System;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using KwikOptions.Options;
using KwikOptions.OptionsProviders;

namespace KwikOptions.Tests
{
    public class KwikOptionsServiceTests
    {
        [Fact]
        public void Should_Invoke_Provider()
        {
            // Given:
            var optionsProvider = new SampleOptionsProvider();

            // And:
            var serviceCollection = new ServiceCollection();

            // And: 
            var rawConfiguration = @"
            {
                ""KwikOptions"": {
                    ""OptionsProviders"": [
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.SampleOptionsProvider, KwikOptions.Tests"",
                            ""Assembly"": ""KwikOptions.Tests.dll""
                        }
                    ]
                },
                ""SampleOptions"": {
                    ""Value"": ""Hello""
                }
            }
            ";

            // And:
            var configuration =
               new ConfigurationBuilder()
                   .AddJsonFile(new InMemoryFileProvider(rawConfiguration), "sample.json", false, false)
                   .Build();

            // And:
            var sut = new KwikOptionsService(serviceCollection, configuration);

            // When:
            sut.InjectOptions();

            // Then:
            var sampleOptions = serviceCollection.BuildServiceProvider().GetServices<IOptions<SampleOptions>>();

            Assert.NotNull(sampleOptions);
        }

        [Fact]
        public void Should_Inject_Only_One_Options()
        {
            // Given:
            var optionsProvider = new SampleOptionsProvider();

            // And:
            var serviceCollection = new ServiceCollection();

            // And: 
            var rawConfiguration = @"
            {
                ""KwikOptions"": {
                    ""OptionsProviders"": [
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.SampleOptionsProvider, KwikOptions.Tests"",
                            ""Assembly"": ""KwikOptions.Tests.dll""
                        },
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.SampleOptionsProvider, KwikOptions.Tests"",
                            ""Assembly"": ""KwikOptions.Tests.dll""
                        }
                    ]
                },
                ""SampleOptions"": {
                    ""Value"": ""Hello""
                }
            }
            ";

            // And:
            var configuration =
               new ConfigurationBuilder()
                   .AddJsonFile(new InMemoryFileProvider(rawConfiguration), "sample.json", false, false)
                   .Build();

            // And:
            var sut = new KwikOptionsService(serviceCollection, configuration);

            // When:
            sut.InjectOptions();

            // Then:
            var sampleOptions = serviceCollection.BuildServiceProvider().GetServices<IOptions<SampleOptions>>();

            Assert.Equal(1, sampleOptions.ToList().Count);
        }

        [Fact]
        public void Should_Inject_Two_Options()
        {
            // Given:
            var optionsProvider = new SampleOptionsProvider();

            // And:
            var serviceCollection = new ServiceCollection();

            // And: 
            var rawConfiguration = @"
            {
                ""KwikOptions"": {
                    ""OptionsProviders"": [
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.SampleOptionsProvider, KwikOptions.Tests"",
                            ""Assembly"": ""KwikOptions.Tests.dll""
                        },
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.AnotherOptionsProvider, KwikOptions.Tests"",
                            ""Assembly"": ""KwikOptions.Tests.dll""
                        }
                    ]
                },
                ""SampleOptions"": {
                    ""Value"": ""Hello""
                }
            }
            ";

            // And:
            var configuration =
               new ConfigurationBuilder()
                   .AddJsonFile(new InMemoryFileProvider(rawConfiguration), "sample.json", false, false)
                   .Build();

            // And:
            var sut = new KwikOptionsService(serviceCollection, configuration);

            // When:
            sut.InjectOptions();

            // Then:
            var sampleOptions = serviceCollection.BuildServiceProvider().GetServices<IOptions<SampleOptions>>();

            Assert.Equal(1, sampleOptions.ToList().Count);
        }
    }

    public class SampleOptionsProvider : BasicOptionsProvider<SampleOptions>
    {
        
    }

    public class AnotherOptionsProvider : BasicOptionsProvider<OtherOptions>
    {

    }

    public class OtherOptions
    {
        public string Value { get; set;}
    }

    public class SampleOptions
    {
        public string Value { get; set; }
    }
}
