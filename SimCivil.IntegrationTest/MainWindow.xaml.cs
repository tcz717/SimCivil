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
// SimCivil - SimCivil.IntegrationTest - MainWindow.xaml.cs
// Create Date: 2018/09/27
// Update Date: 2018/09/28

using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;

using SimCivil.Orleans.Grains.Service;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Service;

namespace SimCivil.IntegrationTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public SimCivil.Gate.Gate GateServer { get; set; }

        public TestCluster Cluster { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeGate()
        {
            GateServer = new SimCivil.Gate.Gate(Cluster.Client);
            var services = new ServiceCollection();
            services.AddLogging(n => n.AddTextBox(GateTextBox));
            GateServer.Run(services).Wait();
        }

        private void InitializeOrleans()
        {
            var builder = new TestClusterBuilder();

            SiloBuilder.LoggerTextBox = ClusterTextBox;
            builder.AddSiloBuilderConfigurator<SiloBuilder>();

            Cluster = builder.Build();
            Cluster.Deploy();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(
                () =>
                {
                    InitializeOrleans();
                    InitializeGate();
                });
        }
    }

    public class SiloBuilder : ISiloBuilderConfigurator
    {
        public static TextBox LoggerTextBox;

        /// <summary>Configures the host builder.</summary>
        public void Configure(ISiloHostBuilder hostBuilder)
        {
            hostBuilder
                .AddMemoryGrainStorageAsDefault()
                .AddStartupTask(
                    (provider, token) => provider.GetRequiredService<IGrainFactory>()
                        .GetGrain<IGame>(0)
                        .InitGame(new Config()))
                .ConfigureLogging(
                    logging => logging.AddDebug().AddTextBox(LoggerTextBox))
                .ConfigureServices(
                    services => { services.AddSingleton<IMapGenerator, RandomMapGen>(); });
        }
    }
}