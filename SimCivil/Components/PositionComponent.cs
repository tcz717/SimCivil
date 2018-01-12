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
// SimCivil - SimCivil - PositionComponent.cs
// Create Date: 2018/01/10
// Update Date: 2018/01/10

using System;
using System.Text;

using Newtonsoft.Json;

namespace SimCivil.Components
{
    /// <summary>
    /// Position Component
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Components.IComponent</cref>
    /// </seealso>
    public class PositionComponent : IComponent
    {
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public float X { get; set; }
        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [JsonIgnore]
        public (float, float) Pos
        {
            get => (X, Y);
            set => (X, Y) = value;
        }

        /// <summary>
        /// Gets the tile x.
        /// </summary>
        /// <value>
        /// The tile x.
        /// </value>
        [JsonIgnore]
        public int TileX => (int) Math.Truncate(X);
        /// <summary>
        /// Gets the tile y.
        /// </summary>
        /// <value>
        /// The tile y.
        /// </value>
        [JsonIgnore]
        public int TileY => (int) Math.Truncate(Y);

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// Gets or sets the entity identifier.
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Clones with specified new identifier.
        /// </summary>
        /// <param name="newId">The new identifier.</param>
        /// <returns></returns>
        public IComponent Clone(Guid newId)
        {
            IComponent comp = (IComponent) MemberwiseClone();
            comp.EntityId = newId;

            return comp;
        }

        /// <summary>
        /// Deconstructs the specified x and y.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void Deconstruct(out float x, out float y)
        {
            x = X;
            y = Y;
        }
    }
}