// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.Test - OrleansFixture.cs
// Create Date: 2018/06/22
// Update Date: 2018/12/19

using System;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;

using SimCivil.Orleans.Grains.Service;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;

using Xunit;

namespace SimCivil.Test.Orleans
{
    [CollectionDefinition(Name)]
    public class ClusterCollection : ICollectionFixture<OrleansFixture>
    {
        public const string Name = "ClusterCollection";
    }

    public class OrleansFixture : IDisposable
    {
        public TestCluster Cluster { get; }

        public OrleansFixture( /*ITestOutputHelper outputHelper*/)
        {
            var builder = new TestClusterBuilder();

            //SiloBuilder.OutputHelper = outputHelper;
            builder.AddSiloBuilderConfigurator<SiloBuilder>();

            Cluster = builder.Build();
            Cluster.Deploy();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Cluster.StopAllSilos();
        }

        public class SiloBuilder : ISiloBuilderConfigurator
        {
            /// <summary>Configures the host builder.</summary>
            public void Configure(ISiloHostBuilder hostBuilder)
            {
                hostBuilder
                    .AddMemoryGrainStorageAsDefault()
                    .ConfigureAppConfiguration(builder => builder.AddJsonFile("appsettings.json"))
                    .AddStartupTask(
                        (provider, token) => provider.GetRequiredService<IGrainFactory>()
                            .GetGrain<IGame>(0)
                            .InitGame())
                    .ConfigureLogging(
                        logging => logging.AddConsole()
                        /*.AddProvider(new XunitLoggerProvider(OutputHelper))*/)
                    .ConfigureServices(
                        (context, services) =>
                        {
                            IConfiguration configuration = context.Configuration;
                            services.AddSingleton<IMapGenerator, RandomMapGen>()
                                .AddSingleton<ITerrainRepository, TestTerrainRepository>()
                                .Configure<GameOptions>(configuration.GetSection("Game"))
                                .Configure<SyncOptions>(configuration.GetSection("Sync"));
                        });
            }

            //public static ITestOutputHelper OutputHelper { get; set; }
        }
    }
}