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
// SimCivil - SimCivil.Orleans.Interfaces - GameOption.cs
// Create Date: 2018/12/13
// Update Date: 2018/12/13

using System;
using System.Text;

namespace SimCivil.Orleans.Interfaces.Option
{
    public class GameOptions
    {
        /// <summary>
        /// Gets or sets the spawn point.
        /// </summary>
        /// <value>
        /// The spawn point.
        /// </value>
        public (int X, int Y) SpawnPoint { get; set; }

        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        /// <value>
        /// The seed.
        /// </value>
        public int Seed { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = "dev";

        /// <summary>Gets or sets the size of the atlas.</summary>
        /// <value>The size of the atlas.</value>
        public int AtlasSize { get; set; } = 64;
        /// <summary>
        /// Gets or sets the sea level.
        /// </summary>
        /// <value>
        /// The sea level.
        /// </value>
        public int SeaLevel { get; set; } = 64;

        /// <summary>Gets or sets a value indicating whether the game is in the development mode.</summary>
        /// <value>
        ///   <c>true</c> if development; otherwise, <c>false</c>.</value>
        public bool Development { get; set; } = true;
    }
}