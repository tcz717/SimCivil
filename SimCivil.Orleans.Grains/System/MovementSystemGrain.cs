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
// SimCivil - SimCivil.Orleans.Grains - MovementSystemGrain.cs
// Create Date: 2018/09/27
// Update Date: 2018/09/27

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.System;

namespace SimCivil.Orleans.Grains.System
{
    public class MovementSystemGrain : Grain, IMovementSystem
    {
        private Dictionary<Guid, (float X, float Y)> _activeEntities;

        /// <summary>
        /// This method is called at the end of the process of activating a grain.
        /// It is called before any messages have been dispatched to the grain.
        /// For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            RegisterTimer(Update, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(50));
            _activeEntities = new Dictionary<Guid, (float X, float Y)>();
            return base.OnActivateAsync();
        }

        private async Task Update(object arg)
        {
            var tasks = _activeEntities.Select(
                async e =>
                {
                    float maxMoveSpeed = (await GrainFactory.Get<IUnit>(e.Key).GetData()).MoveSpeed;
                    await GrainFactory.Get<IPosition>(e.Key)
                        .Add(((int X, int Y)) (maxMoveSpeed * e.Value.X, maxMoveSpeed * e.Value.Y));
                });
            await Task.WhenAll(tasks);
        }

        public Task Move(Guid entityId, (float X, float Y) speed)
        {
            _activeEntities.Add(entityId,speed);
            return Task.CompletedTask;
        }

        public Task Stop(Guid entityId)
        {
            _activeEntities.Remove(entityId);
            return Task.CompletedTask;
        }
    }
}