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
// Create Date: 2018/10/20
// Update Date: 2019/04/13

using System;
using System.Collections.Generic;
using System.Text;

using SimCivil.Contract.Model;

namespace SimCivil.Orleans.Interfaces.Component
{
    public class Unit
    {
        public const int BodyPartCount = (int) BodyPartIndex.BodyPartCount;
        public const int AbilityCount = (int) AbilityIndex.AbilityCount;
        public const int MaxSlotCount = (int) EquipmentSlot.MaxSlotCount;
        /// <summary>Gets or sets the gender.</summary>
        /// <value>The gender.</value>
        public Gender Gender { get; set; }
        /// <summary>Gets or sets the race.</summary>
        /// <value>The race.</value>
        public Race Race { get; set; }

        /// <summary>Gets or sets the equipments.</summary>
        /// <value>The equipments.</value>
        public IEntity[] Equipments { get; set; } = new IEntity[MaxSlotCount];

        #region BodyPart

        public BodyPart[] BodyParts { get; set; } = new BodyPart[BodyPartCount];

        public BodyPart Soul
        {
            get => BodyParts[(int) BodyPartIndex.Soul];
            set => BodyParts[(int) BodyPartIndex.Soul] = value;
        }

        public BodyPart LeftEye
        {
            get => BodyParts[(int) BodyPartIndex.LeftEye];
            set => BodyParts[(int) BodyPartIndex.LeftEye] = value;
        }

        public BodyPart RightEye
        {
            get => BodyParts[(int) BodyPartIndex.RightEye];
            set => BodyParts[(int) BodyPartIndex.RightEye] = value;
        }

        public BodyPart Brain
        {
            get => BodyParts[(int) BodyPartIndex.Brain];
            set => BodyParts[(int) BodyPartIndex.Brain] = value;
        }

        public BodyPart LeftEar
        {
            get => BodyParts[(int) BodyPartIndex.LeftEar];
            set => BodyParts[(int) BodyPartIndex.LeftEar] = value;
        }

        public BodyPart RightEar
        {
            get => BodyParts[(int) BodyPartIndex.RightEar];
            set => BodyParts[(int) BodyPartIndex.RightEar] = value;
        }

        public BodyPart Mouth
        {
            get => BodyParts[(int) BodyPartIndex.Mouth];
            set => BodyParts[(int) BodyPartIndex.Mouth] = value;
        }

        public BodyPart Digestive
        {
            get => BodyParts[(int) BodyPartIndex.Digestive];
            set => BodyParts[(int) BodyPartIndex.Digestive] = value;
        }

        public BodyPart Lung
        {
            get => BodyParts[(int) BodyPartIndex.Lung];
            set => BodyParts[(int) BodyPartIndex.Lung] = value;
        }

        public BodyPart Heart
        {
            get => BodyParts[(int) BodyPartIndex.Heart];
            set => BodyParts[(int) BodyPartIndex.Heart] = value;
        }

        public BodyPart Immunity
        {
            get => BodyParts[(int) BodyPartIndex.Immunity];
            set => BodyParts[(int) BodyPartIndex.Immunity] = value;
        }

        public BodyPart LeftArm
        {
            get => BodyParts[(int) BodyPartIndex.LeftArm];
            set => BodyParts[(int) BodyPartIndex.LeftArm] = value;
        }

        public BodyPart RightArm
        {
            get => BodyParts[(int) BodyPartIndex.RightArm];
            set => BodyParts[(int) BodyPartIndex.RightArm] = value;
        }

        public BodyPart LeftHand
        {
            get => BodyParts[(int) BodyPartIndex.LeftHand];
            set => BodyParts[(int) BodyPartIndex.LeftHand] = value;
        }

        public BodyPart RightHand
        {
            get => BodyParts[(int) BodyPartIndex.RightHand];
            set => BodyParts[(int) BodyPartIndex.RightHand] = value;
        }

        public BodyPart LeftLeg
        {
            get => BodyParts[(int) BodyPartIndex.LeftLeg];
            set => BodyParts[(int) BodyPartIndex.LeftLeg] = value;
        }

        public BodyPart RightLeg
        {
            get => BodyParts[(int) BodyPartIndex.RightLeg];
            set => BodyParts[(int) BodyPartIndex.RightLeg] = value;
        }

        public BodyPart LeftFoot
        {
            get => BodyParts[(int) BodyPartIndex.LeftFoot];
            set => BodyParts[(int) BodyPartIndex.LeftFoot] = value;
        }

        public BodyPart RightFoot
        {
            get => BodyParts[(int) BodyPartIndex.RightFoot];
            set => BodyParts[(int) BodyPartIndex.RightFoot] = value;
        }

        #endregion

        #region Ability

        public UnboundedProperty[] Abilities { get; set; } = new UnboundedProperty[AbilityCount];
        /// <summary>重构能力</summary>
        public UnboundedProperty ReconstructionAbility { get; set; }
        /// <summary>解析能力</summary>
        public UnboundedProperty ResolvingAbility { get; set; }
        /// <summary>感知能力</summary>
        public UnboundedProperty PerceptionAbility { get; set; }
        /// <summary>控制能力</summary>
        public UnboundedProperty ControllingAbility { get; set; }
        /// <summary>精神力</summary>
        public UnboundedProperty MentalAbility { get; set; }
        /// <summary>视力</summary>
        public UnboundedProperty Vision { get; set; }
        /// <summary>创造力</summary>
        public UnboundedProperty Creativity { get; set; }
        /// <summary>记忆力</summary>
        public UnboundedProperty Memory { get; set; }
        /// <summary>理解力</summary>
        public UnboundedProperty Understanding { get; set; }
        /// <summary>意识</summary>
        public UnboundedProperty Consciousness { get; set; }
        /// <summary>消化能力</summary>
        public UnboundedProperty Digestion { get; set; }
        /// <summary>反应力</summary>
        public UnboundedProperty Reaction { get; set; }
        /// <summary>耐力</summary>
        public UnboundedProperty Endurance { get; set; }
        /// <summary>毒素抗性</summary>
        public UnboundedProperty Antitoxic { get; set; }
        /// <summary>上肢力量</summary>
        public UnboundedProperty UpperPower { get; set; }
        /// <summary>下肢力量</summary>
        public UnboundedProperty LowerPower { get; set; }
        /// <summary>上肢灵巧</summary>
        public UnboundedProperty UpperDexterity { get; set; }
        /// <summary>下肢灵巧</summary>
        public UnboundedProperty LowerDexterity { get; set; }

        #endregion

        #region Effect

        /// <summary>移动速率</summary>
        public UnboundedProperty MoveSpeed { get; set; }
        /// <summary>视野距离</summary>
        public UnboundedProperty SightRange { get; set; }

        #endregion
    }

    public class BodyPart
    {
        public uint MaxHp { get; set; }
        public uint Hp { get; set; }
        public uint Potentiality { get; set; }
        public List<Wound> Wounds { get; set; } = new List<Wound>();
        public float Efficiency => (float) Hp * Potentiality / MaxHp;

        public static BodyPart Create(uint potentiality, uint maxHp) => new BodyPart
        {
            Potentiality = potentiality,
            Hp = maxHp,
            MaxHp = maxHp,
        };

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Hp: {Hp}/{MaxHp} ({Efficiency}), P: {Potentiality}, W: {Wounds.Count}";
        }
    }

    // TODO Wound
    public class Wound { }

    public enum BodyPartIndex
    {
        Soul,
        LeftEye,
        RightEye,
        Brain,
        LeftEar,
        RightEar,
        Mouth,
        Digestive,
        Lung,
        Heart,
        Immunity,
        LeftArm,
        RightArm,
        LeftHand,
        RightHand,
        LeftLeg,
        RightLeg,
        LeftFoot,
        RightFoot,
        BodyPartCount
    }

    public enum AbilityIndex
    {
        /// <summary>重构能力</summary>
        ReconstructionAbility,

        /// <summary>解析能力</summary>
        ResolvingAbility,

        /// <summary>感知能力</summary>
        PerceptionAbility,

        /// <summary>控制能力</summary>
        ControllingAbility,

        /// <summary>精神力</summary>
        MentalAbility,

        /// <summary>视力</summary>
        Vision,

        /// <summary>创造力</summary>
        Creativity,

        /// <summary>记忆力</summary>
        Memory,

        /// <summary>理解力</summary>
        Understanding,

        /// <summary>意识</summary>
        Consciousness,

        /// <summary>消化能力</summary>
        Digestion,

        /// <summary>反应力</summary>
        Reaction,

        /// <summary>耐力</summary>
        Endurance,

        /// <summary>毒素抗性</summary>
        Antitoxic,

        /// <summary>上肢力量</summary>
        UpperPower,

        /// <summary>下肢力量</summary>
        LowerPower,

        /// <summary>上肢灵巧</summary>
        UpperDexterity,

        /// <summary>下肢灵巧</summary>
        LowerDexterity,
        AbilityCount
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

        public float Value => Base * (1 + ScaleBonus) + LinearBonus;

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

        /// <summary>Performs an implicit conversion from <see cref="UnboundedProperty"/> to <see cref="float"/>.</summary>
        /// <param name="property">The property.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float(UnboundedProperty property)
        {
            return property.Value;
        }

        /// <summary>Applies the linear bonus.</summary>
        /// <param name="linearBonus">The linear bonus.</param>
        public UnboundedProperty ApplyLinearBonus(float linearBonus)
            => new UnboundedProperty(Base, LinearBonus + linearBonus, ScaleBonus);

        /// <summary>Applies the scale bonus.</summary>
        /// <param name="scaleBonus">The scale bonus.</param>
        public UnboundedProperty ApplyScaleBonus(float scaleBonus)
            => new UnboundedProperty(Base, LinearBonus, ScaleBonus + scaleBonus);

        public UnboundedProperty Update(float @base) => new UnboundedProperty(@base, LinearBonus, ScaleBonus);

        /// <summary>Resets the linear bonus.</summary>
        public UnboundedProperty ResetLinearBonus() => new UnboundedProperty(Base, 0, ScaleBonus);

        /// <summary>Resets the scale bonus.</summary>
        public UnboundedProperty ResetScaleBonus() => new UnboundedProperty(Base, LinearBonus, 0);

        /// <summary>Resets this instance.</summary>
        public UnboundedProperty Reset() => new UnboundedProperty(Base);

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString()
        {
            return $"{Value:N} ({Base:N} * {(1 + ScaleBonus):N} + {LinearBonus:N})";
        }
    }
}