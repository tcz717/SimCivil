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
// Create Date: 2018/12/13
// Update Date: 2018/12/30

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;
using SimCivil.Orleans.Interfaces.System;

namespace SimCivil.Orleans.Grains.Component
{
    public class ControllerGrain : Grain, IUnitController
    {
        private readonly ITerrainRepository _terrainRepository;
        private double _lagPredict;
        private DateTime _lastUpdateTime;
        private IMovementSystem _moveSystem;
        private (float X, float Y) _speed;

        public TimeSpan UpdatePeriod { get; set; }

        public ILogger<ControllerGrain> Logger { get; }
        public IOptions<GameOptions> GameOptions { get; }
        public IOptions<SyncOptions> SyncOptions { get; }

        public ControllerGrain(
            ILogger<ControllerGrain> logger,
            IConfiguration configuration,
            ITerrainRepository terrainRepository,
            IOptions<GameOptions> gameOptions,
            IOptions<SyncOptions> syncOptions)
        {
            Logger = logger;
            GameOptions = gameOptions;
            SyncOptions = syncOptions;
            _terrainRepository = terrainRepository;
            UpdatePeriod = TimeSpan.FromMilliseconds(configuration.GetValue(nameof(UpdatePeriod), 50));
            _lastUpdateTime = DateTime.Now - UpdatePeriod - UpdatePeriod;
        }

        public Task<(float X, float Y)> GetSpeed()
        {
            return Task.FromResult(_speed);
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

        /// <summary>
        /// Moves to.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public async Task MoveTo(Position position, DateTime timeStamp)
        {
            double delta = (timeStamp - DateTime.UtcNow).TotalMilliseconds;
            _lagPredict = SyncOptions.Value.LagLearningRate * delta +
                          (1 - SyncOptions.Value.LagLearningRate) * _lagPredict;
            DateTime predictTimeStamp = timeStamp.AddMilliseconds(-_lagPredict);
            if (Math.Abs(delta - _lagPredict) > SyncOptions.Value.MaxLag)
            {
                Logger.LogWarning(
                    $"Entity{this.GetPrimaryKey()} has abnormal timespan {delta - _lagPredict:N1} in {nameof(MoveTo)}");

                throw new ArgumentOutOfRangeException(nameof(timeStamp));
            }

            Position previous = await GrainFactory.Get<IPosition>(this).GetData();
            Tile tile = await GrainFactory.GetTile(previous.Tile, GameOptions);
            TimeSpan dt = predictTimeStamp - _lastUpdateTime;

            _lastUpdateTime = predictTimeStamp;

            if (dt > SyncOptions.Value.MaxUpdateDelta)
                dt = SyncOptions.Value.MaxUpdateDelta;

            double dx2 = (position.X - previous.X) * (position.X - previous.X) +
                         (position.Y - previous.Y) * (position.Y - previous.Y);

            double v2 = dx2 / (dt.TotalSeconds * dt.TotalSeconds);
            float speed = await GrainFactory.Get<IUnit>(this).GetMoveSpeed();
            double maxv2 = _terrainRepository.GetTerrain(tile.Terrain).BaseMoveSpeed * speed;
            maxv2 *= maxv2;

            if (v2 > maxv2 + 0.001)
            {
                var alpha = (float) Math.Sqrt(maxv2 / v2);
                if (alpha < 0.9)
                    Logger.LogWarning(
                        $"{this.GetPrimaryKey()} moves too fast: {nameof(alpha)} = {alpha}, expected = {position}");

                if (SyncOptions.Value.NoSpeedFix)
                    return;

                position.X = previous.X + alpha * (position.X - previous.X);
                position.Y = previous.Y + alpha * (position.Y - previous.Y);
            }

            // TODO check if construction exists
            if (_terrainRepository.GetTerrain((await GrainFactory.GetTile(position.Tile, GameOptions)).Terrain)
                .Flags.HasFlag(TerrainFlags.NotMovable))
                return;

            await GrainFactory.Get<IPosition>(this).SetData(position);
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

        public async Task<InspectionResult> InspectEntity(IEntity target)
        {
            var result = new InspectionResult
            {
                EntityId = target.GetPrimaryKey(),
                ObserverId = this.GetPrimaryKey(),
                TimeStamp = DateTime.UtcNow,
                Values = new Dictionary<string, string>()
            };

            var components = await GrainFactory.GetEntity(this).GetComponents();
            var inspections = await Task.WhenAll(
                components.Where(c => !(c is IUnitController)).Select(c => c.Inspect(target)));
            result.Values = inspections.Where(i => i != null).SelectMany(i => i).ToDictionary(i => i.Key, i => i.Value);

            return result;
        }

        public Task<IComponent> CopyTo(IEntity target)
        {
            return Task.FromResult((IComponent) GrainFactory.Get<IUnitController>(target));
        }

        public Task<IReadOnlyDictionary<string, string>> Dump()
        {
            return Task.FromResult(
                (IReadOnlyDictionary<string, string>) new Dictionary<string, string>
                {
                    ["Speed"] = _speed.ToString()
                });
        }

        public Task<IReadOnlyDictionary<string, string>> Inspect(IEntity target)
        {
            return Dump();
        }

        public Task Delete()
        {
            return Task.CompletedTask;
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
    }
}