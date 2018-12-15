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
// Create Date: 2018/06/14
// Update Date: 2018/12/15

using System;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

using Sentry;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Option;

namespace SimCivil.Orleans.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (SentrySdk.Init("https://c091709188504c39a331cc91794fa4f4@sentry.io/216217"))
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
                            .AddCommandLine(args))
                    .ConfigureServices(Configure);

                ISiloHost silo = siloBuilder.Build();
                await silo.StartAsync();

                AssemblyLoadContext.Default.Unloading += context =>
                {
                    Console.WriteLine("Signal kill detected");
                    silo.StopAsync().Wait();
                };

                await silo.Stopped;
            }
        }

        private static void Configure(HostBuilderContext context, IServiceCollection serviceCollection)
        {
            IConfiguration configuration = context.Configuration;
            serviceCollection
                .AddLogging(
                    logging => logging.AddConsole()
                        .AddConfiguration(configuration.GetSection("Logging")))
                .Configure<EndpointOptions>(configuration.GetSection("Endpoint"))
                .Configure<ClusterOptions>(configuration.GetSection("Cluster"))
                .Configure<GameOptions>(configuration.GetSection("Game"))
                .Configure<SyncOptions>(configuration.GetSection("Sync"));
        }
    }
}