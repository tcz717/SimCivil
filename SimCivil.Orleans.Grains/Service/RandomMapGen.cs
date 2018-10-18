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
// SimCivil - SimCivil.Orleans.Grains - RandomMapGen.cs
// Create Date: 2018/02/26
// Update Date: 2018/02/26

using System;
using System.Text;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Service;

namespace SimCivil.Orleans.Grains.Service
{
    /// <inheritdoc />
    /// <summary>
    /// Random map generator used for test.
    /// </summary>
    public class RandomMapGen : IMapGenerator
    {
        /// <summary>
        /// Generate new Atlas
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="x">Index of the atlas</param>
        /// <param name="y">Index of the atlas</param>
        /// <param name="width">Atlas's Width</param>
        /// <param name="height">Atlas's Height</param>
        /// <returns></returns>
        public Tile[,] Generate(int seed, int x, int y, int width, int height)
        {
            Random rand = new Random(seed * x + y);
            Tile[,] tiles = new Tile[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[i, j] = Tile.Create(
                        (x * width + i, y * height + j),
                        height: rand.Next(Config.SeaLevel - 10, Config.SeaLevel + 10));
                }
            }

            return tiles;
        }
    }
}