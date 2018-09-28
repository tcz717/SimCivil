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
// Create Date: 2018/06/14
// Update Date: 2018/09/28

using System;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Orleans;

using SharpRaven;
using SharpRaven.Data;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.System;
using SimCivil.Rpc;

namespace SimCivil.Gate
{
    public class Gate
    {
        public static RavenClient Raven { get; } =
            new RavenClient("https://c091709188504c39a331cc91794fa4f4@sentry.io/216217");

        public IClusterClient Client { get; set; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public Gate([NotNull] IClusterClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        private static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            IClusterClient client = new ClientBuilder().UseLocalhostClustering().Build();

            await client.Connect();

            await new Gate(client).Run();

            Console.ReadKey();
        }

        public async Task Run(IServiceCollection services = null)
        {
            var container = LoadConfiguration(services);
            container.RegisterInstance(Client.ServiceProvider.GetService<IGrainFactory>());
            RpcServer server = new RpcServer(container.Build())
            {
#if DEBUG
                Debug = true
#endif
            };

            await server.Bind(20170).Run();
            server.Container.Resolve<OrleansChunkViewSynchronizer>().StartSync(server);

            await PrepareTest();
        }

        private async Task PrepareTest()
        {
            var factory = Client.ServiceProvider.GetService<IGrainFactory>();
            var logger = Client.ServiceProvider.GetService<ILogger<Gate>>();
            logger.LogInformation(
                $"Register {await factory.GetGrain<IAccount>("admin").Register("")}");

            var human = factory.GetGrain<IEntity>(Guid.NewGuid());
            await human.Add<IObserver>();
            await human.Add<IPosition>();
            await human.Add<IUnit>();
            await human.Add<IUnitController>();

            await factory.GetGrain<IPrefabManager>(0).Set("human.init", human);
            await factory.GetGrain<IGame>(0).InitGame(new Config {SpawnPoint = (0, 0), Seed = 0});
        }

        private static ContainerBuilder LoadConfiguration(
            IServiceCollection services = null,
            string config = "configuration.json")
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

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Raven.Capture(new SentryEvent((Exception) e.ExceptionObject));
        }
    }
}