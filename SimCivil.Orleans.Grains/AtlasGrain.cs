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
// SimCivil - SimCivil.Orleans.Grains - AtlasGrain.cs
// Create Date: 2018/06/14
// Update Date: 2018/10/07

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Service;

using static System.Math;

using static SimCivil.Orleans.Interfaces.Config;

namespace SimCivil.Orleans.Grains
{
    public class AtlasGrain : Grain<AtlasState>, IAtlas
    {
        public IMapGenerator Generator { get; }
        public ILogger<AtlasGrain> Logger { get; }
        public (int X, int Y) AtlasIndex { get; set; }
        public int Left => AtlasIndex.X * DefaultAtlasSize;

        /// <summary>
        /// The maximum X position of it's tiles.
        /// </summary>
        public int Right => AtlasIndex.X * DefaultAtlasSize + DefaultAtlasSize;

        /// <summary>
        /// The minimum Y position of it's tiles.
        /// </summary>
        public int Top => AtlasIndex.Y * DefaultAtlasSize;

        /// <summary>
        /// The maximum Y position of it's tiles.
        /// </summary>
        public int Bottom => AtlasIndex.Y * DefaultAtlasSize + DefaultAtlasSize;

        public AtlasGrain(IMapGenerator generator, ILogger<AtlasGrain> logger)
        {
            Generator = generator;
            Logger = logger;
        }

        public Task<IEnumerable<Tile>> SelectRange((int X, int Y) leftTop, int width, int height)
        {
            List<Tile> tiles = new List<Tile>();
            int right = Min(Right, leftTop.X + width);
            int bottom = Min(Bottom, leftTop.Y + height);
            int left = Max(Left, leftTop.X);
            int top = Max(Top, leftTop.Y);
            for (int i = left; i < right; i++)
            for (int j = top; j < bottom; j++)
                tiles.Add(State[i - Left, j - Top]);

            return Task.FromResult((IEnumerable<Tile>) tiles);
        }

        public Task SetTile((int X, int Y) pos, Tile tile)
        {
            if (pos.X >= Right || pos.X < Left || pos.Y >= Bottom || pos.Y < Top)
                throw new ArgumentOutOfRangeException(nameof(pos));

            State[pos.X - Left, pos.Y - Top] = tile;


            return Task.CompletedTask;
        }

        public Task<Tile> GetTile((int X, int Y) pos)
        {
            if (pos.X >= Right || pos.X < Left || pos.Y >= Bottom || pos.Y < Top)
                throw new ArgumentOutOfRangeException(nameof(pos));

            return Task.FromResult(State[pos.X - Left, pos.Y - Top]);
        }

        public Task<Tile[,]> Dump()
        {
            return Task.FromResult(State.Tiles);
        }

        public override async Task OnActivateAsync()
        {
            long id = this.GetPrimaryKeyLong();
            AtlasIndex = ((int) (id & 0xFFFF_FFFF), (int) (id >> 32));
            Debug.WriteLine(State);
            if (State?.Tiles == null)
            {
                State = new AtlasState
                {
                    Tiles = Generator.Generate(
                        (await GrainFactory.GetGrain<IGame>(0).GetConfig()).Seed,
                        AtlasIndex.X,
                        AtlasIndex.Y,
                        DefaultAtlasSize)
                };

                if (State.Tiles.Length != DefaultAtlasSize * DefaultAtlasSize)
                    throw new ArgumentOutOfRangeException(nameof(State.Tiles));

                Logger.Info($"Atlas {AtlasIndex} was created.");
            }
        }
    }

    public class AtlasState
    {
        public Tile[,] Tiles { get; set; }

        public Tile this[int x, int y]
        {
            get => Tiles[x, y];
            set => Tiles[x, y] = value;
        }
    }

    public static class AtlasExtension
    {
        public static async Task<Tile> GetTile(this IGrainFactory factory, (int X, int Y) position)
        {
            return await factory.GetGrain<IAtlas>(position.DivDown(DefaultAtlasSize)).GetTile(position);
        }
    }
}