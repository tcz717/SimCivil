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
// SimCivil - SimCivil.Orleans.Grains - BaseGrain.cs
// Create Date: 2018/06/22
// Update Date: 2018/10/06

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans;

using SimCivil.Orleans.Interfaces;

namespace SimCivil.Orleans.Grains.Component
{
    public abstract class BaseGrain<TComponent> : Grain<TComponent>, IComponent<TComponent> where TComponent : new()
    {
        public ILogger Logger { get; }

        protected BaseGrain(ILoggerFactory factory)
        {
            Logger = factory.CreateLogger(GetType());
        }

        public virtual Task<TComponent> GetData()
        {
            StateCheck();

            return Task.FromResult(State);
        }

        public virtual Task SetData(TComponent component)
        {
            State = component;

            return Task.CompletedTask;
        }

        public virtual async Task<IComponent> CopyTo(IEntity target)
        {
            StateCheck();
            Type type = GetType()
                           .GetInterfaces()
                           .FirstOrDefault(
                               t => t != typeof(IComponent<TComponent>) &&
                                    typeof(IComponent<TComponent>).IsAssignableFrom(t))
                       ?? typeof(IComponent<TComponent>);
            var component = GrainFactory.GetGrain<IComponent<TComponent>>(type, target.GetPrimaryKey());

            await component.SetData(State);

            return component;
        }

        public virtual Task<IReadOnlyDictionary<string, string>> Dump()
        {
            StateCheck();

            return Task.FromResult(
                (IReadOnlyDictionary<string, string>) State.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name, prop => prop.GetValue(State, null)?.ToString()));
        }

        public virtual Task<IReadOnlyDictionary<string, string>> Inspect(IEntity observer)
        {
            return Task.FromResult<IReadOnlyDictionary<string, string>>(null);
        }

        public Task Delete()
        {
            return ClearStateAsync();
        }

        private void StateCheck()
        {
            if (State == null)
            {
                Logger.LogWarning("Access null state");
            }
        }
    }
}