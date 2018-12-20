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
// SimCivil - SimCivil.Orleans.Interfaces - Unit.cs
// Create Date: 2018/10/21
// Update Date: 2018/12/19

using System;
using System.Text;

using SimCivil.Contract.Model;

namespace SimCivil.Orleans.Interfaces.Component
{
    public class Unit
    {
        /// <summary>Gets or sets the gender.</summary>
        /// <value>The gender.</value>
        public Gender Gender { get; set; }
        /// <summary>Gets or sets the race.</summary>
        /// <value>The race.</value>
        public Race Race { get; set; }

        /// <summary>Gets or sets the move speed.</summary>
        /// <value>The move speed.</value>
        public UnboundedProperty MoveSpeed { get; set; }

        /// <summary>Gets or sets the sight range.</summary>
        /// <value>The sight range.</value>
        public UnboundedProperty SightRange { get; set; }

        /// <summary>Gets or sets the equipments.</summary>
        /// <value>The equipments.</value>
        public IEntity[] Equipments { get; set; } = new IEntity[MaxSlotCount];

        public const int MaxSlotCount = (int) EquipmentSlot.MaxSlotCount;
    }
    
    public enum EquipmentSlot
    {
        Helmet,
        Mask,
        Necklace,
        Shoulder,
        Belt,
        Armor,
        Backpack,
        PrimaryWeapon,
        SecondaryWeapon,
        Gloves,
        Pants,
        Shoes,
        LeftBracelet,
        RightBracelet,
        Ring1,
        Ring2,
        MaxSlotCount,
    }

    /// <summary xml:lang="cn">表示一种无最大值约束的属性，可以被施加线性和倍率加成</summary>
    public struct UnboundedProperty
    {
        public float Base { get; set; }
        public float LinearBonus { get; set; }
        public float ScaleBonus { get; set; }

        public float Value => Base * ScaleBonus + LinearBonus;

        /// <summary>Initializes a new instance of the <see cref="UnboundedProperty"/> struct.</summary>
        /// <param name="init">The initialize base.</param>
        public UnboundedProperty(float init) : this()
        {
            Base = init;
        }

        /// <summary>Initializes a new instance of the <see cref="UnboundedProperty"/> struct.</summary>
        /// <param name="base">The base.</param>
        /// <param name="linearBonus">The linear bonus.</param>
        /// <param name="scaleBonus">The scale bonus.</param>
        public UnboundedProperty(float @base, float linearBonus, float scaleBonus)
        {
            Base = @base;
            LinearBonus = linearBonus;
            ScaleBonus = scaleBonus;
        }

        /// <summary>Performs an implicit conversion from <see cref="UnboundedProperty"/> to <see cref="System.Single"/>.</summary>
        /// <param name="property">The property.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float(UnboundedProperty property)
        {
            return property.Value;
        }

        /// <summary>Applies the linear bonus.</summary>
        /// <param name="linearBonus">The linear bonus.</param>
        public void ApplyLinearBonus(float linearBonus) => LinearBonus += linearBonus;

        /// <summary>Applies the scale bonus.</summary>
        /// <param name="scaleBonus">The scale bonus.</param>
        public void ApplyScaleBonus(float scaleBonus) => ScaleBonus += scaleBonus;

        /// <summary>Resets the linear bonus.</summary>
        public void ResetLinearBonus() => LinearBonus = 0;

        /// <summary>Resets the scale bonus.</summary>
        public void ResetScaleBonus() => ScaleBonus = 0;

        /// <summary>Resets this instance.</summary>
        public void Reset()
        {
            ResetLinearBonus();
            ResetScaleBonus();
        }
    }
}