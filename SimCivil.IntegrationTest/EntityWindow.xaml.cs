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
// SimCivil - SimCivil.IntegrationTest - EntityWindow.xaml.cs
// Create Date: 2018/10/04
// Update Date: 2018/10/05

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Microsoft.Extensions.DependencyInjection;

using Orleans;

using SimCivil.Orleans.Interfaces;

namespace SimCivil.IntegrationTest
{
    /// <summary>
    /// EntityWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EntityWindow : Window
    {
        private readonly IEntity _entity;
        private readonly IGrainFactory _factory;
        private DispatcherTimer _timer;
        public IServiceProvider ServiceProvider { get; }
        public Guid EntityId { get; }

        public EntityWindow(IServiceProvider serviceProvider, Guid entityId)
        {
            ServiceProvider = serviceProvider;
            EntityId = entityId;
            _factory = serviceProvider.GetService<IClusterClient>().ServiceProvider.GetService<IGrainFactory>();
            _entity = _factory.GetGrain<IEntity>(EntityId);
            InitializeComponent();
        }

        private async void EntityWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Title = Task.Factory.StartNew(() => _entity.GetName().Result).Result;
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
            IdText.Content = EntityId;
            NameText.Content = await _entity.GetName();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            var components = await _entity.GetComponents();
            var table = await Task.Factory.StartNew(
                () =>
                    components.SelectMany(
                            c => c.Dump().Result,
                            DisplayNameFormat)
                        .ToDictionary(
                            p => p.Key,
                            p => p.Value
                        ));
            EntityDataGrid.ItemsSource = table;
        }

        private static KeyValuePair<string, string> DisplayNameFormat(IComponent c, KeyValuePair<string, string> prop)
        {
            Type type = null;
            foreach (Type t in c.GetType()
                .GetInterfaces())
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IComponent<>))
                {
                    type = t.GetGenericArguments().First();

                    break;
                }

                if (t.Namespace != null && t.Namespace.Contains("Component"))
                {
                    type = t;

                    break;
                }
            }

            string typeName = type?.Name ?? c.GetType().Name;


            return new KeyValuePair<string, string>($"{typeName}.{prop.Key}", prop.Value);
        }
    }
}