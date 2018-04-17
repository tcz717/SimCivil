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
// Create Date: 2018/02/24
// Update Date: 2018/02/24

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Orleans.Grains.State;
using SimCivil.Orleans.Interfaces;

namespace SimCivil.Orleans.Grains
{
    public class EntityGrain : Grain<EntityState>, IEntity
    {
        public Task<bool> Has<T>() where T : IComponent
        {
            return Task.FromResult(State.ComponentNames.Contains(typeof(T).FullName));
        }

        public Task Add<T>() where T : IComponent
        {
            State.ComponentNames.Add(typeof(T).FullName);

            return Task.CompletedTask;
        }

        public Task Remove<T>() where T : IComponent
        {
            State.ComponentNames.Remove(typeof(T).FullName);

            return Task.CompletedTask;
        }

        public async Task Enable()
        {
            if (State.Enabled) return;
            await GrainFactory.GetGrain<IEntityGroup>(0).AddEntity(this.GetPrimaryKey());
            await Task.WhenAll(
                State.ComponentNames.Select(
                    c => GrainFactory.GetGrain<IEntityGroup>(c.GetHashCode()).AddEntity(this.GetPrimaryKey())));
            State.Enabled = true;
        }

        public async Task Disable()
        {
            if (!State.Enabled) return;
            await GrainFactory.GetGrain<IEntityGroup>(0).RemoveEntity(this.GetPrimaryKey());
            await Task.WhenAll(
                State.ComponentNames.Select(
                    c => GrainFactory.GetGrain<IEntityGroup>(c.GetHashCode()).RemoveEntity(this.GetPrimaryKey())));
            State.Enabled = false;
        }

        public Task<bool> IsEnabled()
        {
            return Task.FromResult(State.Enabled);
        }

        public override async Task OnActivateAsync()
        {
            if (State == null)
            {
                State = new EntityState();
                await Enable();
            }

            await base.OnActivateAsync();
        }
    }
}