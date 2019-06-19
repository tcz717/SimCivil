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
// SimCivil - SimCivil.Orleans.Grains - MapService.cs
// Create Date: 2019/05/31
// Update Date: 2019/05/31

using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Orleans;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;
using SimCivil.Utilities.AutoService;

namespace SimCivil.Orleans.Grains.Service
{
    [AutoService(ServiceLifetime.Transient)]
    public class MapService : IMapService
    {
        public IGrainFactory         GrainFactory      { get; }
        public ITerrainRepository    TerrainRepository { get; }
        public IOptions<GameOptions> GameOptions       { get; }

        public MapService(
            IGrainFactory         grainFactory,
            ITerrainRepository    terrainRepository,
            IOptions<GameOptions> gameOptions)
        {
            GrainFactory      = grainFactory;
            TerrainRepository = terrainRepository;
            GameOptions       = gameOptions;
        }

        public IAtlas GetAtlas((int X, int Y) position)
            => GrainFactory.GetGrain<IAtlas>(position.DivDown(GameOptions.Value.AtlasSize));

        public Task<Tile> GeTile((int X, int Y) position) => GetAtlas(position).GetTile(position);

        public async Task<Terrain> GetTerrain((int X, int Y) position)
            => TerrainRepository.GetTerrain((await GeTile(position)).Terrain);

        public async Task<float> GetEntityActualMaxSpeed(IEntity entity)
        {
            PositionState position = await GrainFactory.Get<IPosition>(entity).GetData();
            var unit = GrainFactory.Get<IUnit>(entity);
            return await unit.GetMoveSpeed() * (await GetTerrain(position.Tile)).BaseMoveSpeed;
        }

        public async Task<float> CalculateDistance(IEntity entity1, IEntity entity2)
        {
            var positions = await Task.WhenAll(GrainFactory.Get<IPosition>(entity1).GetData(), GrainFactory.Get<IPosition>(entity2).GetData());
            float d2 = (positions[0].X - positions[1].X) * (positions[0].X - positions[1].X) +
                       (positions[0].Y - positions[1].Y) * (positions[0].Y - positions[1].Y);

            return (float) Math.Sqrt(d2);
        }
    }
}