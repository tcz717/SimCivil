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
// SimCivil - SimCivil.Orleans.Grains - ObserverGrain.cs
// Create Date: 2018/06/14
// Update Date: 2018/06/17

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;

namespace SimCivil.Orleans.Grains.Component
{
    public class ObserverGrain : BaseGrain<Observer>, IObserver
    {
        public HashSet<Guid> Entities { get; set; }

        public Task OnEntityEntered(Guid id)
        {
            Entities.Add(id);

            return Task.CompletedTask;
        }

        public Task OnEntityLeft(Guid id)
        {
            Entities.Remove(id);

            return Task.CompletedTask;
        }

        public Task<uint> GetNotifyRange()
        {
            return Task.FromResult(State.NotityRange);
        }

        public Task<IEnumerable<Guid>> PopAllEntities()
        {
            var entities = Entities.ToArray();
            Entities.Clear();

            return Task.FromResult((IEnumerable<Guid>) entities);
        }

        /// <summary>
        /// This method is called at the end of the process of activating a grain.
        /// It is called before any messages have been dispatched to the grain.
        /// For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            Entities = new HashSet<Guid>();

            return base.OnActivateAsync();
        }

        public async Task<ViewChange> UpdateView()
        {
            var viewChange = new ViewChange();

            var entities = await Task.WhenAll(
                Entities.Select(
                    async e =>
                    {
                        var dto = new EntityDto();
                        var entity = GrainFactory.GetGrain<IEntity>(e);

                        dto.Name = await entity.GetName();

                        return dto;
                    }));
            Entities.Clear();

            viewChange.EntityChange = entities;

            return viewChange;
        }
    }
}