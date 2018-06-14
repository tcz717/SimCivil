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
// SimCivil - SimCivil.Gate - Program.cs
// Create Date: 2018/06/14
// Update Date: 2018/06/14

using System;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using Autofac.Configuration;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Orleans;

using SharpRaven;
using SharpRaven.Data;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Rpc;

namespace SimCivil.Gate
{
    class Program
    {
        public static RavenClient Raven { get; } =
            new RavenClient("https://c091709188504c39a331cc91794fa4f4@sentry.io/216217");

        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            IClusterClient client = new ClientBuilder().UseLocalhostClustering().Build();

            await client.Connect();

            var container = LoadConfiguration();
            container.RegisterInstance(client.ServiceProvider.GetService<IGrainFactory>());
            RpcServer server = new RpcServer(container.Build())
            {
#if DEBUG
                Debug = true
#endif
            };

            await server.Bind(20170).Run();

            Console.Out.WriteLine(
                $"register {await client.ServiceProvider.GetService<IGrainFactory>().GetGrain<IAccount>("admin").Register("")}");

            Console.ReadKey();
        }

        private static ContainerBuilder LoadConfiguration(string config = "configuration.json")
        {
            var builder = new ContainerBuilder();
            var configBuilder = new ConfigurationBuilder();
//            configBuilder.AddJsonFile(config);
            var module = new ConfigurationModule(configBuilder.Build());

            var services = new ServiceCollection();
            services.AddLogging(n => n.AddConsole());

            builder.RegisterModule(module);
            builder.Populate(services);
            builder.UseRpcSession();
            builder.RegisterRpcProvider<OrleansAuth, IAuth>().SingleInstance();
            builder.RegisterRpcProvider<OrleansRoleManager, IRoleManager>().InstancePerChannel();
//            builder.RegisterRpcProvider<OrleansChunkViewSynchronizer, IViewSynchronizer>().SingleInstance();
//            builder.RegisterRpcProvider<OrleansPlayerController, IPlayerController>().InstancePerChannel();

            return builder;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Raven.Capture(new SentryEvent((Exception) e.ExceptionObject));
        }
    }
}