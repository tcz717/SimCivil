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
// Create Date: 2018/02/25
// Update Date: 2018/05/14

using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans.Configuration;
using Orleans.Hosting;

using SharpRaven;
using SharpRaven.Data;

namespace SimCivil.Orleans.Server
{
    class Program
    {
        public static RavenClient Raven { get; } =
            new RavenClient("https://c091709188504c39a331cc91794fa4f4@sentry.io/216217");

        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            ISiloHostBuilder siloBuilder = new SiloHostBuilder()
                .UseLocalhostClustering(clusterId: "tpdt-dev")
                .AddMemoryGrainStorageAsDefault()
                .ConfigureLogging(
                    logging => logging.AddConsole());
            ISiloHost silo = siloBuilder.Build();
            await silo.StartAsync();

            Console.WriteLine("Press Enter to close.");
            // wait here
            Console.ReadLine();

            // shut the silo down after we are done.
            await silo.StopAsync();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Raven.Capture(new SentryEvent((Exception) e.ExceptionObject));
        }
    }
}