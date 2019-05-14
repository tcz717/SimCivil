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
// SimCivil - SimCivil.Orleans.Interfaces - UnitProperty.cs
// Create Date: 2019/05/13
// Update Date: 2019/05/14

using System;
using System.Text;

namespace SimCivil.Orleans.Interfaces.Component.State
{
    /// <summary xml:lang="cn">表示一种无最大值约束的属性，可以被施加线性和倍率加成</summary>
    public class UnitProperty : IComparable<UnitProperty>
    {
        public int CompareTo(UnitProperty other) => Value.CompareTo(other.Value);

        public float Base        { get; set; }
        public float LinearBonus { get; set; }
        public float ScaleBonus  { get; set; }

        public float Value => Base * (1 + ScaleBonus) + LinearBonus;

        /// <summary>Initializes a new instance of the <see cref="UnitProperty" /> struct.</summary>
        /// <param name="init">The initialize base.</param>
        public UnitProperty(float init)
        {
            Base = init;
        }

        /// <summary>Initializes a new instance of the <see cref="UnitProperty" /> struct.</summary>
        /// <param name="base">The base.</param>
        /// <param name="linearBonus">The linear bonus.</param>
        /// <param name="scaleBonus">The scale bonus.</param>
        public UnitProperty(float @base, float linearBonus, float scaleBonus)
        {
            Base        = @base;
            LinearBonus = linearBonus;
            ScaleBonus  = scaleBonus;
        }

        public UnitProperty() { }

        /// <summary>Performs an implicit conversion from <see cref="float" /> to <see cref="float" />.</summary>
        /// <param name="property">The property.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float(UnitProperty property) => property.Value;

        /// <summary>Applies the linear bonus.</summary>
        /// <param name="linearBonus">The linear bonus.</param>
        public UnitProperty ApplyLinearBonus(float linearBonus)
        {
            LinearBonus += linearBonus;

            return this;
        }

        /// <summary>Applies the scale bonus.</summary>
        /// <param name="scaleBonus">The scale bonus.</param>
        public UnitProperty ApplyScaleBonus(float scaleBonus)
        {
            ScaleBonus += scaleBonus;

            return this;
        }

        public UnitProperty Update(float @base)
        {
            Base = @base;

            return this;
        }

        /// <summary>Resets the linear bonus.</summary>
        public UnitProperty ResetLinearBonus()
        {
            LinearBonus = 0;

            return this;
        }

        /// <summary>Resets the scale bonus.</summary>
        public UnitProperty ResetScaleBonus()
        {
            ScaleBonus = 0;

            return this;
        }

        /// <summary>Resets this instance.</summary>
        public UnitProperty Reset() => ResetLinearBonus().ResetScaleBonus();

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() => $"{Value:N} ({Base:N} * {1 + ScaleBonus:N} + {LinearBonus:N})";
    }
}