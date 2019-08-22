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
// SimCivil - SimCivil.Orleans.Interfaces - Tile.cs
// Create Date: 2018/06/14
// Update Date: 2018/10/07

using System;
using System.Text;

namespace SimCivil.Orleans.Interfaces
{
    public class Tile
    {
        /// <summary>
        /// Tile's height.
        /// Default sea level is  SimCivil.Config.SeaLevel .
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the terrain.
        /// </summary>
        /// <value>
        /// The terrain.
        /// </value>
        public int Terrain { get; set; }

        /// <summary>
        /// Tile's Position.
        /// </summary>
        public (int X, int Y) Position { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the item on the tile.
        /// </summary>
        /// <value>
        /// The item unique identifier.
        /// </value>
        public Guid ItemGuid { get; set; }

        /// <summary>
        /// Method to create a new Tile
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="terrain"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Tile Create((int X, int Y) pos, int terrain = 0, int height = 0)
        {
            var e = new Tile
            {
                Height = height,
                Position = pos,
                Terrain = terrain,
            };

            return e;
        }
    }
}