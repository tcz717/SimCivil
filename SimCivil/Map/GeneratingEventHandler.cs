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
// SimCivil - SimCivil - GeneratingEventHandler.cs
// Create Date: 2018/01/07
// Update Date: 2018/01/08

using System;
using System.Text;

namespace SimCivil.Map
{
    /// <summary>
    /// Represent a new atlas generating.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class GeneratingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public (int X, int Y) Position { get; set; }

        /// <summary>
        /// Gets or sets the tile.
        /// </summary>
        /// <value>
        /// The tile.
        /// </value>
        public Tile Tile { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SimCivil.Map.GeneratingEventArgs" /> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="tile">The tile.</param>
        public GeneratingEventArgs((int X, int Y) position, Tile tile)
        {
            Position = position;
            Tile = tile;
        }
    }

    /// <summary>
    /// When a new atlas is generating.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="GeneratingEventArgs"/> instance containing the event data.</param>
    public delegate void GeneratingEventHandler(IMapGenerator sender, GeneratingEventArgs args);
}