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
// Create Date: 2018/02/25
// Update Date: 2018/02/25

using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Orleans.Interfaces
{
    public class Tile
    {
        /// <summary>
        /// Tile's height.
        /// Defalut sea level is  SimCivil.Config.SeaLevel .
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Tile's surface texture.
        /// </summary>
        public string Surface { get; set; }

        /// <summary>
        /// Extra data about Tile.
        /// </summary>
        public Dictionary<string, object> Meta { get; set; }

        /// <summary>
        /// Tile's Position.
        /// </summary>
        public (int X, int Y) Position { get; set; }

        /// <summary>
        /// Gets or sets the item unique identifier.
        /// </summary>
        /// <value>
        /// The item unique identifier.
        /// </value>
        public Guid ItemGuid { get; set; }

        /// <summary>
        /// Method to craete a new Tile
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="surface"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Tile Create((int X, int Y) pos, string surface = "", int height = 0)
        {
            var e = new Tile
            {
                Height = height,
                Meta = new Dictionary<string, object>(),
                Position = pos,
                Surface = surface,
            };

            return e;
        }
    }
}