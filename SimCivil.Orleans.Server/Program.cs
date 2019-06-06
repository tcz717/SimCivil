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
// Create Date: 2019/06/04
// Update Date: 2019/06/05

using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;

using Sentry;

using SimCivil.Orleans.Grains;
using SimCivil.Orleans.Interfaces;
using SimCivil.Utilities.AutoService;

using EnvironmentName = Microsoft.Extensions.Hosting.EnvironmentName;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;

namespace SimCivil.Orleans.Server
{
    internal class Program
    {
        private const string Dsn = "https://c091709188504c39a331cc91794fa4f4@sentry.io/216217";

        public static async Task Main(string[] args)
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
                                            .ConfigureEndpoints(
                                                 Dns.GetHostName(),
                                                 11111,
                                                 30000,
                                                 listenOnAnyHostAddress: true)
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
                                             false)
                                        .AddJsonFile(
                                             $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                                             true)
                                        .AddEnvironmentVariables("SC_")
                                        .AddCommandLine(args))
                            .ConfigureLogging(ConfigureLogging)
                            .ConfigureServices(
                                 (context, collection) =>
                                     collection.AutoService(typeof(GameGrain).Assembly)
                                               .AutoOptions(typeof(GameGrain).Assembly, context.Configuration)
                                               .Configure<EndpointOptions>(
                                                    context.Configuration.GetSection("Endpoint"))
                                               .Configure<ClusterOptions>(
                                                    context.Configuration.GetSection("Cluster"))
                                               .Configure<DynamoDBClusteringOptions>(
                                                    context.Configuration.GetSection("DynamoDBClustering")))
                            .Build();

                try
                {
                    await host.StartAsync();

                    await host.WaitForShutdownAsync();
                }
                catch (OrleansConfigurationException configurationException)
                {
                    host.Services.GetService<ILogger<Program>>()
                        .LogCritical(configurationException, "Configuration missing");
                }
            }
        }

        private static void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
            builder.AddConsole()
                   .AddSentry(Dsn)
                   .AddConfiguration(context.Configuration.GetSection("Logging"));

            if (context.HostingEnvironment.IsDevelopment())
                builder.AddFile(
                    o =>
                    {
                        o.BasePath       = "logs";
                        o.EnsureBasePath = true;
                        o.FallbackFileName =
                            $"server-{Assembly.GetExecutingAssembly().GetName().Version}-{DateTime.Now:yyyy-dd-M-HH-mm-ss}.log";
                    });
        }
    }
}