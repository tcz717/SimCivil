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
// Update Date: 2018/12/13

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Extensions.Configuration;
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

        public ServiceProvider ClientServices { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeGate()
        {
            GateServer = new SimCivil.Gate.Gate(Cluster.Client);
            var services = new ServiceCollection();
            services.AddLogging(
                n => n.AddLogViewer(GateLogViewer)
                    .AddFilter(level => level > LogLevel.Debug));
            GateServer.Run(services).Wait();
        }

        private void InitializeOrleans()
        {
            var builder = new TestClusterBuilder();

            SiloBuilder.LoggerProvider = new WpfLogProvider(ClusterLogViewer);
            builder.AddSiloBuilderConfigurator<SiloBuilder>();

            Cluster = builder.Build();
            Cluster.Deploy();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(
                () =>
                {
                    InitializeOrleans();
                    InitializeGate();
                });
            LoadTestcases();
        }

        private void LoadTestcases()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(
                n => n.AddLogViewer(ClientLogViewer)
                    .AddFilter(level => level > LogLevel.Debug));
            Debug.Assert(Cluster.Client != null, "Cluster.Client != null");
            serviceCollection.AddSingleton(Cluster.Client);

            var types = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(type => typeof(IIntegrationTest).IsAssignableFrom(type) && type.IsClass)
                .ToArray();

            foreach (Type type in types)
            {
                serviceCollection.AddTransient(type);
            }

            ClientServices = serviceCollection.BuildServiceProvider();

            foreach (Type type in types)
            {
                var button = new Button {Content = type.Name};
                button.Click += delegate
                {
                    var test = (IIntegrationTest) ClientServices.GetRequiredService(type);
                    ((TestcaseCollection) RunningStack.DataContext).Add(test);
                    test.Test()
                        .ContinueWith(
                            t =>
                                ClientServices.GetService<ILogger<MainWindow>>()
                                    .LogError(t.Exception, $"{type.Name} fails"),
                            TaskContinuationOptions.OnlyOnFaulted);
                };
                TestcaseStackPanel.Children.Add(button);
            }
        }

        private void EntityDetailButton_OnClick(object sender, RoutedEventArgs e)
        {
            var testcase = (IIntegrationTest) ((Control) sender).DataContext;
            EntityWindow window = new EntityWindow(ClientServices, testcase.GetEntityId());
            window.Show();
        }

        private async void StopMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var testcase = (IIntegrationTest) ((Control) sender).DataContext;
            await testcase.Stop();
            ((TestcaseCollection) RunningStack.DataContext).Remove(testcase);
        }
    }

    public class SiloBuilder : ISiloBuilderConfigurator
    {
        public static WpfLogProvider LoggerProvider;

        /// <summary>Configures the host builder.</summary>
        public void Configure(ISiloHostBuilder hostBuilder)
        {
            hostBuilder
                .AddMemoryGrainStorageAsDefault()
                .ConfigureAppConfiguration(
                    (context, configure) => configure
                        .AddJsonFile(
                            "appsettings.json",
                            optional: false)
                        .AddJsonFile(
                            $"appsettings.{context.HostingEnvironment}.json",
                            optional: true))
                .AddStartupTask(
                    (provider, token) => provider.GetRequiredService<IGrainFactory>()
                        .GetGrain<IGame>(0)
                        .InitGame(true))
                .ConfigureLogging(
                    logging => logging.AddDebug().AddProvider(LoggerProvider))
                .ConfigureServices(
                    services =>
                    {
                        services.AddSingleton<IMapGenerator, RandomMapGen>()
                            .AddSingleton<ITerrainRepository, TestTerrainRepository>();
                    });
        }
    }

    public class TestcaseCollection : ObservableCollection<IIntegrationTest> { }
}