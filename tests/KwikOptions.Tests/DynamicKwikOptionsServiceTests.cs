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
    public class DynamicKwikOptionsServiceTests
    {
        [Fact]
        public void Should_Invoke_Provider()
        {
            // Given:
            var serviceCollection = new ServiceCollection();

            // And: 
            var rawConfiguration = @"
            {
                ""KwikOptions"": {
                    ""OptionsTypes"": [
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.SampleOptions, KwikOptions.Tests"",
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
            var options = new RootKwikOptions();
            configuration.GetSection("KwikOptions").Bind(options);

            // And:
            var sut = new DynamicKwikOptionsService(serviceCollection, configuration, options);

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
            var serviceCollection = new ServiceCollection();

            // And: 
            var rawConfiguration = @"
            {
                ""KwikOptions"": {
                    ""OptionsTypes"": [
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.SampleOptions, KwikOptions.Tests"",
                            ""Assembly"": ""KwikOptions.Tests.dll""
                        },
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.SampleOptions, KwikOptions.Tests"",
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
            var options = new RootKwikOptions();
            configuration.GetSection("KwikOptions").Bind(options);

            // And:
            var sut = new DynamicKwikOptionsService(serviceCollection, configuration, options);

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
            var serviceCollection = new ServiceCollection();

            // And: 
            var rawConfiguration = @"
            {
                ""KwikOptions"": {
                    ""OptionsTypes"": [
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.SampleOptions, KwikOptions.Tests"",
                            ""Assembly"": ""KwikOptions.Tests.dll""
                        },
                        {
                            ""OptionsPath"": ""SampleOptions"",
                            ""Type"": ""KwikOptions.Tests.OtherOptions, KwikOptions.Tests"",
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
            var options = new RootKwikOptions();
            configuration.GetSection("KwikOptions").Bind(options);

            // And:
            var sut = new DynamicKwikOptionsService(serviceCollection, configuration, options);

            // When:
            sut.InjectOptions();

            // Then:
            var sampleOptions = serviceCollection.BuildServiceProvider().GetServices<IOptions<SampleOptions>>();

            Assert.Equal(1, sampleOptions.ToList().Count);
        }
    }
}
