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
// SimCivil - SimCivil.Orleans.Interfaces - Position.cs
// Create Date: 2018/06/22
// Update Date: 2018/09/26

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SimCivil.Orleans.Interfaces.Component
{
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }
        public (int X, int Y) Tile => ((int) Math.Truncate(X), (int) Math.Truncate(Y));
        public static Position Zero { get; } = new Position();

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public Position() { }

        public Position((int X, int Y) pos) : this(pos.X, pos.Y) { }

        public static implicit operator (float X, float Y)(Position position)
        {
            return (position.X, position.Y);
        }

        public static Position operator +(Position pos, (float X, float Y) offset)
        {
            pos.X += offset.X;
            pos.Y += offset.Y;

            return pos;
        }

        protected bool Equals(Position other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Position) obj);
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
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