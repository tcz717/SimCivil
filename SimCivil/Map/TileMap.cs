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
// SimCivil - SimCivil - MapData.cs
// Create Date: 2017/08/18
// Update Date: 2018/01/02

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using log4net;

using SimCivil.Store;

using static SimCivil.Config;

namespace SimCivil.Map
{
    /// <summary>
    ///     Map consists of Atlas.
    /// </summary>
    public class TileMap
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TileMap));

        /// <summary>
        ///     Gets the atlas collection.
        /// </summary>
        /// <value>
        ///     The atlas collection.
        /// </value>
        public Dictionary<(int X, int Y), Atlas> AtlasCollection { get; } = new Dictionary<(int X, int Y), Atlas>();

        /// <summary>
        ///     Gets the map generator.
        /// </summary>
        /// <value>
        ///     The map generator.
        /// </value>
        public IMapGenerator MapGenerator { get; }

        /// <summary>
        ///     Gets the map repository.
        /// </summary>
        /// <value>
        ///     The map repository.
        /// </value>
        public IMapRepository MapRepository { get; }

        /// <summary>
        ///     Whether allowing expanding when tile not exists.
        /// </summary>
        public bool AllowExpanding { get; set; } = true;


        /// <summary>
        ///     Get tile by position.
        /// </summary>
        /// <param name="x">Tile's X position</param>
        /// <param name="y">Tile's Y position</param>
        /// <returns>Tile you want.</returns>
        /// <exception cref="IndexOutOfRangeException">Tile isn't existed and expanding is not allowed.</exception>
        public Tile this[int x, int y]
        {
            get
            {
                (int X, int Y) atlasIndex = Pos2AtlasIndex((x, y));
                Atlas atlas;

                if (AtlasCollection.ContainsKey(atlasIndex))
                {
                    atlas = AtlasCollection[atlasIndex];
                }
                else if (MapRepository.Contains(atlasIndex))
                {
                    atlas = MapRepository.GetAtlas(atlasIndex);
                    AtlasCollection[atlasIndex] = atlas;
                    Logger.Info($"Loaded Atlas {atlasIndex} with {MapRepository}.");
                }
                else if (AllowExpanding)
                {
                    atlas = MapGenerator.Generate(atlasIndex.X, atlasIndex.Y);
                    AtlasCollection[atlasIndex] = atlas;
                    MapRepository.PutAtlas(atlasIndex, atlas);
                    Logger.Info($"Generated Atlas {atlasIndex} with {MapGenerator}.");
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }

                return atlas.Tiles[
                    ModPositive(x, DefaultAtlasWidth),
                    ModPositive(y, DefaultAtlasHeight)
                ];
            }
        }

        /// <summary>
        ///     Get tile by position.
        /// </summary>
        /// <param name="pos">Tile's position</param>
        /// <returns>Tile you want.</returns>
        /// <exception cref="IndexOutOfRangeException">Tile isn't existed and expanding is not allowed.</exception>
        public Tile this[(int X, int Y) pos] => this[pos.X, pos.Y];

        /// <summary>
        ///     Config a map data container.
        /// </summary>
        /// <param name="mapGenerator">Object to generate new atlas.</param>
        /// <param name="mapRepository">Object to generate load existed atlas.</param>
        public TileMap(IMapGenerator mapGenerator, IMapRepository mapRepository)
        {
            MapGenerator = mapGenerator;
            MapRepository = mapRepository;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ModPositive(int a, int b)
        {
            return (a % b + b) % b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DivDown(int a, int b)
        {
            return (a - (a % b + b) % b) / b;
        }

        private static (int X, int Y) Pos2AtlasIndex((int x, int y) pos)
        {
            return (DivDown(pos.x, DefaultAtlasWidth), DivDown(pos.y, DefaultAtlasHeight));
        }

        /// <summary>
        /// Selects the range.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="viewDistance">The view distance.</param>
        /// <returns></returns>
        public IEnumerable<Tile> SelectRange((int x, int y) position, uint viewDistance)
        {
            (long x, long y) lt = (position.x - viewDistance, position.y - viewDistance),
                lb = (position.x - viewDistance, position.y + viewDistance),
                rt = (position.x + viewDistance, position.y - viewDistance);

            for (int i = (int) lt.y; i < lb.y; i++)
            for (int j = (int) lt.x; j < rt.x; j++)
                yield return this[i, j];
        }
    }
}