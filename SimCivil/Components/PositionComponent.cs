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
// Update Date: 2018/02/04

using System;
using System.Text;

using Newtonsoft.Json;

using SimCivil.Model;

namespace SimCivil.Components
{
    /// <summary>
    /// Position Component
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Components.IComponent</cref>
    /// </seealso>
    public class PositionComponent : BaseComponent
    {
        private (float X, float Y) _toSync;
        private float _x;
        private float _y;

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{nameof(Pos)}: {Pos}, {nameof(PreviousPos)}: {PreviousPos}";
        }

        /// <summary>
        /// Gets the previous position.
        /// </summary>
        /// <value>
        /// The previous position.
        /// </value>
        [JsonIgnore]
        public (float X, float Y) PreviousPos { get; private set; }

        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public float X
        {
            get => _x;
            set
            {
                if (value.Equals(_x)) return;

                _x = value;
                OnPropertyChanged(nameof(Pos));
            }
        }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public float Y
        {
            get => _y;
            set
            {
                if (value.Equals(_y)) return;

                _y = value;
                OnPropertyChanged(nameof(Pos));
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        [JsonIgnore]
        public (float X, float Y) Pos
        {
            get => (X, Y);
            set
            {
                if (value.Equals((_x, _y))) return;

                (_x, _y) = value;
                OnPropertyChanged();
            }
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

        /// <summary>
        /// Synchronic the previous position.
        /// </summary>
        /// <returns></returns>
        public (float X, float Y) Sync()
        {
            PreviousPos = _toSync;
            _toSync = Pos;

            return PreviousPos;
        }
    }

    public static partial class EntityExtenstion
    {
        private static readonly string PositionName = typeof(PositionComponent).FullName;
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static PositionComponent GetPos(this Entity entity)
        {

            return (PositionComponent) entity[PositionName];
        }
    }
}