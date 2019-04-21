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
// Update Date: 2019/04/20

using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

using Sentry;

using SimCivil.Orleans.Grains.Service;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;

using EnvironmentName = Microsoft.Extensions.Hosting.EnvironmentName;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace SimCivil.Orleans.Server
{
    class Program
    {
        private const string Dsn = "https://c091709188504c39a331cc91794fa4f4@sentry.io/216217";

        static async Task Main(string[] args)
        {
            using (SentrySdk.Init(Dsn))
            {
                IHost host = new HostBuilder()
                    .UseEnvironment(EnvironmentName.Development)
                    .UseOrleans(
                        (context, builder) =>
                        {
                            builder.AddMemoryGrainStorageAsDefault()
                                .UseDynamoDBClustering((Action<DynamoDBClusteringOptions>) null)
                                .Configure<EndpointOptions>(
                                    options => options.AdvertisedIPAddress = Dns.GetHostAddresses(Dns.GetHostName())[0])
                                .AddStartupTask(
                                    (provider, token) => provider.GetRequiredService<IGrainFactory>()
                                        .GetGrain<IGame>(0)
                                        .InitGame());
                        })
                    .ConfigureAppConfiguration(
                        (context, builder) =>
                            builder
                                .AddJsonFile(
                                    "appsettings.json",
                                    optional: false)
                                .AddJsonFile(
                                    $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                                    optional: true)
                                .AddEnvironmentVariables("SC_")
                                .AddCommandLine(args))
                    .ConfigureLogging(ConfigureLogging)
                    .ConfigureServices(ConfigureOption)
                    .ConfigureServices(ConfigureServices)
                    .Build();

                await host.StartAsync();

                await host.WaitForShutdownAsync();
            }
        }

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IMapGenerator, RandomMapGen>()
                .AddSingleton<ITerrainRepository, TestTerrainRepository>()
                .AddTransient<IUnitGenerator, TestUnitGenerator>();
        }

        private static void ConfigureOption(HostBuilderContext context, IServiceCollection services)
        {
            IConfiguration configuration = context.Configuration;

            services
                .Configure<EndpointOptions>(configuration.GetSection("Endpoint"))
                .Configure<ClusterOptions>(configuration.GetSection("Cluster"))
                .Configure<GameOptions>(configuration.GetSection("Game"))
                .Configure<SyncOptions>(configuration.GetSection("Sync"))
                .Configure<DynamoDBClusteringOptions>(configuration.GetSection("DynamoDBClustering"));
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            builder.AddConsole()
                .AddSentry(Dsn)
                .AddConfiguration(context.Configuration.GetSection("Logging"));
        }
    }
}