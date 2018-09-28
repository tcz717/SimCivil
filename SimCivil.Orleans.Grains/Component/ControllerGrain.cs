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
// SimCivil - SimCivil.Orleans.Grains - ControllerGrain.cs
// Create Date: 2018/09/26
// Update Date: 2018/09/26

using System;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.System;

namespace SimCivil.Orleans.Grains.Component
{
    public class ControllerGrain : Grain, IUnitController
    {
        private (float X, float Y) _speed;
        private IMovementSystem _moveSystem;

        public Task<(float X, float Y)> GetSpeed()
        {
            return Task.FromResult(_speed);
        }

        /// <summary>
        /// This method is called at the end of the process of activating a grain.
        /// It is called before any messages have been dispatched to the grain.
        /// For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            _moveSystem = GrainFactory.GetGrain<IMovementSystem>(0);

            return base.OnActivateAsync();
        }

        /// <summary>
        /// Moves the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public async Task Move((float X, float Y) direction, float speed)
        {
            if (speed < 0 || speed > 1) throw new ArgumentOutOfRangeException(nameof(speed));

            direction = Utils.Normalize(direction);
            if (Math.Abs(direction.X) < 0.001 && Math.Abs(direction.Y) < 0.001)
            {
                await _moveSystem.Stop(this.GetPrimaryKey());

                return;
            }
            
            _speed = (direction.X * speed, direction.Y * speed);
            await _moveSystem.Move(this.GetPrimaryKey(), _speed);
        }

        public async Task Stop()
        {
            await _moveSystem.Stop(this.GetPrimaryKey());
        }

        public Task Drop(IEntity target)
        {
            throw new NotImplementedException();
        }

        public Task Attack(IEntity target)
        {
            throw new NotImplementedException();
        }

        public Task Use(IEntity target)
        {
            throw new NotImplementedException();
        }

        public Task<EntityInspection> Inspect(IEntity target)
        {
            throw new NotImplementedException();
        }

        public Task CopyTo(IEntity target)
        {
            return Task.CompletedTask;
        }
    }
}