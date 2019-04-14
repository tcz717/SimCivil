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
// SimCivil - SimCivil.Orleans.Server - Program.cs
// Create Date: 2018/06/13
// Update Date: 2019/04/13

using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

using Sentry;

using SimCivil.Orleans.Grains.Service;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;

namespace SimCivil.Orleans.Server
{
    class Program
    {
        private const string Dsn = "https://c091709188504c39a331cc91794fa4f4@sentry.io/216217";

        static async Task Main(string[] args)
        {
            using (SentrySdk.Init(Dsn))
            {
                ISiloHostBuilder siloBuilder = new SiloHostBuilder()
                    .UseLocalhostClustering()
                    .AddMemoryGrainStorageAsDefault()
                    .AddStartupTask(
                        (provider, token) => provider.GetRequiredService<IGrainFactory>()
                            .GetGrain<IGame>(0)
                            .InitGame())
                    .ConfigureAppConfiguration(
                        (context, configure) => configure
                            .AddJsonFile(
                                "appsettings.json",
                                optional: false)
                            .AddJsonFile(
                                $"appsettings.{context.HostingEnvironment}.json",
                                optional: true)
                            .AddEnvironmentVariables()
                            .AddCommandLine(args))
                    .ConfigureServices(Configure);

                ISiloHost silo = siloBuilder.Build();
                await silo.StartAsync();

                var closeEvent = new AutoResetEvent(false);
                Console.CancelKeyPress += (sender, e) => { closeEvent.Reset(); };
                closeEvent.WaitOne();
                silo.Services.GetService<ILogger<Program>>().LogInformation("stopping");
                await silo.StopAsync();
            }
        }

        private static void Configure(HostBuilderContext context, IServiceCollection serviceCollection)
        {
            IConfiguration configuration = context.Configuration;
            serviceCollection
                .AddLogging(
                    logging => logging.AddConsole()
                        .AddSentry(Dsn)
                        .AddConfiguration(configuration.GetSection("Logging")))
                .Configure<EndpointOptions>(configuration.GetSection("Endpoint"))
                .Configure<ClusterOptions>(configuration.GetSection("Cluster"))
                .Configure<GameOptions>(configuration.GetSection("Game"))
                .Configure<SyncOptions>(configuration.GetSection("Sync"));

            serviceCollection.PostConfigure<EndpointOptions>(
                options =>
                {
                    options.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, options.GatewayPort);
                    options.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, options.SiloPort);
                });

            serviceCollection.AddSingleton<IMapGenerator, RandomMapGen>()
                .AddSingleton<ITerrainRepository, TestTerrainRepository>()
                .AddTransient<IUnitGenerator, TestUnitGenerator>();
        }
    }
}