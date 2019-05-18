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
// SimCivil - SimCivil.Orleans.Grains - EntityGrain.cs
// Create Date: 2019/05/13
// Update Date: 2019/05/18

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Orleans;
using Orleans.Runtime;

using SimCivil.Orleans.Grains.State;
using SimCivil.Orleans.Interfaces;

namespace SimCivil.Orleans.Grains
{
    [UsedImplicitly]
    public class EntityGrain : Grain<EntityState>, IEntity
    {
        public Task<bool> Has<T>() where T : IComponent => Task.FromResult(
            State.Components.Contains(GrainFactory.GetGrain<T>(this.GetPrimaryKey())));

        public Task<T> Get<T>() where T : class, IComponent
        {
            var component = GrainFactory.GetGrain<T>(this.GetPrimaryKey());

            return State.Components.Contains(component) ? Task.FromResult(component) : Task.FromResult<T>(null);
        }

        public Task Add<T>() where T : IComponent
        {
            State.Components.Add(GrainFactory.GetGrain<T>(this.GetPrimaryKey()));

            return WriteStateAsync();
        }

        public async Task Remove<T>() where T : IComponent
        {
            var component = GrainFactory.GetGrain<T>(this.GetPrimaryKey());
            State.Components.Remove(component);
            await component.Delete();
            await WriteStateAsync();
        }

        public Task<bool> IsExisted() => Task.FromResult(State != null);

        public async Task Enable()
        {
            if (State.Enabled) return;

            await GrainFactory.GetGrain<IEntityGroup>(0).AddEntity(this.GetPrimaryKey());

            await Task.WhenAll(
                State.Components.OfType<GrainReference>()
                     .Select(
                          c => GrainFactory.GetGrain<IEntityGroup>(c.InterfaceName.GetHashCode())
                                           .AddEntity(this.GetPrimaryKey())));
            State.Enabled = true;
            await WriteStateAsync();
        }

        public async Task Disable()
        {
            if (!State.Enabled) return;

            await GrainFactory.GetGrain<IEntityGroup>(0).RemoveEntity(this.GetPrimaryKey());
            await Task.WhenAll(
                State.Components.Select(
                    c => GrainFactory.GetGrain<IEntityGroup>(c.GetHashCode())
                                     .RemoveEntity(this.GetPrimaryKey())));
            State.Enabled = false;
            await WriteStateAsync();
        }

        public Task<bool> IsEnabled() => Task.FromResult(State.Enabled);

        public async Task CopyTo(IEntity targetEntity)
        {
            var components = await Task.WhenAll(
                                 State.Components.Select(
                                           component => component.CopyTo(targetEntity))
                                      .ToArray());
            await targetEntity.SetComponents(components);
        }

        public Task SetName(string name)
        {
            State.Name = name;

            return WriteStateAsync();
        }

        public Task<string> GetName() => Task.FromResult(State.Name);

        public Task<IReadOnlyCollection<IComponent>> GetComponents()
            => Task.FromResult((IReadOnlyCollection<IComponent>) State.Components);

        public Task SetComponents(IEnumerable<IComponent> components)
        {
            var collection = components as IComponent[] ?? components.ToArray();

            if (collection.Any(component => component.GetPrimaryKey() != this.GetPrimaryKey()))
                throw new ArgumentException("component key not match", nameof(components));

            State.Components = new HashSet<IComponent>(collection);

            return WriteStateAsync();
        }

        public async Task Delete()
        {
            await Task.WhenAll(State.Components.Select(c => c.Delete()));
            await ClearStateAsync();
            State = null;
            DeactivateOnIdle();
        }
    }
}