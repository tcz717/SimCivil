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
// Create Date: 2018/10/21
// Update Date: 2018/12/13

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.System;

namespace SimCivil.Orleans.Grains.System
{
    public class MovementSystemGrain : Grain, IMovementSystem
    {
        private Dictionary<Guid, (float X, float Y)> _activeEntities;
        public ILogger<MovementSystemGrain> Logger { get; }
        public IOptions<GameOptions> GameOptions { get; }

        public int UpdatePeriod { get; set; }

        public MovementSystemGrain(
            ILogger<MovementSystemGrain> logger,
            IOptions<GameOptions> gameOptions,
            IOptions<SyncOptions> syncOptions)
        {
            Logger = logger;
            GameOptions = gameOptions;
            UpdatePeriod = syncOptions.Value.UpdatePeriod;
        }

        public Task Move(Guid entityId, (float X, float Y) speed)
        {
            _activeEntities.Add(entityId, speed);

            return Task.CompletedTask;
        }

        public Task Stop(Guid entityId)
        {
            _activeEntities.Remove(entityId);

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method is called at the end of the process of activating a grain.
        /// It is called before any messages have been dispatched to the grain.
        /// For grains with declared persistent state, this method is called after the State property has been populated.
        /// </summary>
        public override Task OnActivateAsync()
        {
            RegisterTimer(Update, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(UpdatePeriod));
            _activeEntities = new Dictionary<Guid, (float X, float Y)>();

            return base.OnActivateAsync();
        }

        private async Task Update(object arg)
        {
            var tasks = _activeEntities.Select(MoveEntity);
            await Task.WhenAll(tasks);
        }

        private async Task MoveEntity(KeyValuePair<Guid, (float X, float Y)> e)
        {
            float maxMoveSpeed = await GrainFactory.Get<IUnit>(e.Key).GetMoveSpeed();
            var position = GrainFactory.Get<IPosition>(e.Key);
            Position pos = await position.GetData();
            Tile currentTile = await GrainFactory.GetTile(pos.Tile, GameOptions);

            //TODO find a better way to handle collapse
        }

//        private IEnumerable<(int X, int Y)> Bresenham((float X, float Y) start, (float X, float Y) end)
//        {
//            bool steep = Abs(end.X - start.X) <= Abs(end.Y - start.Y);
//            if (steep)
//            {
//                (start.X, start.Y) = (start.Y, start.X);
//                (end.X, end.Y) = (end.Y, end.X);
//            }
//
//            if (start.X > end.X)
//            {
//                (start, end) = (end, start);
//            }
//        }
    }
}