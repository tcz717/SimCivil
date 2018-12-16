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
// SimCivil - SimCivil.Gate - Gate.cs
// Create Date: 2018/12/15
// Update Date: 2018/12/15

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime;

using Sentry;

using SimCivil.Contract;
using SimCivil.Rpc;

namespace SimCivil.Gate
{
    public class Gate
    {
        public IClusterClient Client { get; set; }

        public RpcServer Server { get; set; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public Gate([NotNull] IClusterClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        private static async Task Main(string[] args)
        {
            using (SentrySdk.Init("https://c091709188504c39a331cc91794fa4f4@sentry.io/216217"))
            {
                IClusterClient client = new ClientBuilder()
                    .UseLocalhostClustering()
                    .Configure<NetworkingOptions>(options => options.OpenConnectionTimeout = TimeSpan.FromSeconds(10))
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
                    .ConfigureServices(Configure)
                    .Build();

                try
                {
                    await client.Connect();

                    var gate = new Gate(client);
                    await gate.Run();

                    var closeEvent = new AutoResetEvent(false);
                    Console.CancelKeyPress += (sender, e) => { closeEvent.Reset(); };
                    closeEvent.WaitOne();
                    client.ServiceProvider.GetService<ILogger<Gate>>().LogInformation("stopping");
                    gate.Server.Stop();
                    await client.Close();
                }
                catch (SiloUnavailableException)
                {
                    client.ServiceProvider.GetService<ILogger<Gate>>().LogCritical("Silo connecting fails");
                }
            }
        }

        private static void Configure(HostBuilderContext context, IServiceCollection serviceCollection)
        {
            IConfiguration configuration = context.Configuration;
            serviceCollection
                .AddLogging(
                    logging => logging.AddConsole()
                        .AddConfiguration(configuration.GetSection("Logging")))
                .Configure<ClusterOptions>(configuration.GetSection("Cluster"));
        }

        public async Task Run(IServiceCollection services = null)
        {
            var container = ConfigureRpc(services);
            container.RegisterInstance(Client.ServiceProvider.GetService<IGrainFactory>());
            Server = new RpcServer(container.Build())
            {
#if DEBUG
                Debug = true
#endif
            };

            await Server.Bind(20170).Run();
            Server.Container.Resolve<OrleansChunkViewSynchronizer>().StartSync(Server);
        }

        private static ContainerBuilder ConfigureRpc(
            IServiceCollection services = null)
        {
            var builder = new ContainerBuilder();
            var configBuilder = new ConfigurationBuilder();
//            configBuilder.AddJsonFile(config);
            var module = new ConfigurationModule(configBuilder.Build());

            if (services == null)
            {
                services = new ServiceCollection();
            }

            services.AddLogging(n => n.AddConsole());

            builder.RegisterModule(module);
            builder.Populate(services);
            builder.UseRpcSession();
            builder.RegisterRpcProvider<OrleansAuth, IAuth>().SingleInstance();
            builder.RegisterRpcProvider<OrleansRoleManager, IRoleManager>().InstancePerChannel();
            builder.RegisterRpcProvider<OrleansChunkViewSynchronizer, IViewSynchronizer>().SingleInstance().AsSelf();
            builder.RegisterRpcProvider<OrleansPlayerController, IPlayerController>().InstancePerChannel();

            return builder;
        }
    }
}