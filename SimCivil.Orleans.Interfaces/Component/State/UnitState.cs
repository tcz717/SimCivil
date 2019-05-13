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
// Create Date: 2019/05/08
// Update Date: 2019/05/12

using System;
using System.Collections.Generic;
using System.Text;

using SimCivil.Contract.Model;

namespace SimCivil.Orleans.Interfaces.Component
{
    public class UnitState
    {
        public const int BodyPartCount = (int) BodyPartIndex.BodyPartCount;
        public const int AbilityCount  = (int) AbilityIndex.AbilityCount;
        public const int EffectCount   = (int) EffectIndex.EffectCount;
        public const int MaxSlotCount  = (int) EquipmentSlot.MaxSlotCount;
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
        public UnboundedProperty ReconstructionAbility
        {
            get => Abilities[(int) AbilityIndex.ReconstructionAbility];
            set => Abilities[(int) AbilityIndex.ReconstructionAbility] = value;
        }

        /// <summary>解析能力</summary>
        public UnboundedProperty ResolvingAbility
        {
            get => Abilities[(int) AbilityIndex.ResolvingAbility];
            set => Abilities[(int) AbilityIndex.ResolvingAbility] = value;
        }

        /// <summary>感知能力</summary>
        public UnboundedProperty PerceptionAbility
        {
            get => Abilities[(int) AbilityIndex.PerceptionAbility];
            set => Abilities[(int) AbilityIndex.PerceptionAbility] = value;
        }

        /// <summary>控制能力</summary>
        public UnboundedProperty ControllingAbility
        {
            get => Abilities[(int) AbilityIndex.ControllingAbility];
            set => Abilities[(int) AbilityIndex.ControllingAbility] = value;
        }

        /// <summary>精神力</summary>
        public UnboundedProperty MentalAbility
        {
            get => Abilities[(int) AbilityIndex.MentalAbility];
            set => Abilities[(int) AbilityIndex.MentalAbility] = value;
        }

        /// <summary>视力</summary>
        public UnboundedProperty Vision
        {
            get => Abilities[(int) AbilityIndex.Vision];
            set => Abilities[(int) AbilityIndex.Vision] = value;
        }

        /// <summary>创造力</summary>
        public UnboundedProperty Creativity
        {
            get => Abilities[(int) AbilityIndex.Creativity];
            set => Abilities[(int) AbilityIndex.Creativity] = value;
        }

        /// <summary>记忆力</summary>
        public UnboundedProperty Memory
        {
            get => Abilities[(int) AbilityIndex.Memory];
            set => Abilities[(int) AbilityIndex.Memory] = value;
        }

        /// <summary>理解力</summary>
        public UnboundedProperty Understanding
        {
            get => Abilities[(int) AbilityIndex.Understanding];
            set => Abilities[(int) AbilityIndex.Understanding] = value;
        }

        /// <summary>意识</summary>
        public UnboundedProperty Consciousness
        {
            get => Abilities[(int) AbilityIndex.Consciousness];
            set => Abilities[(int) AbilityIndex.Consciousness] = value;
        }

        /// <summary>消化能力</summary>
        public UnboundedProperty Digestion
        {
            get => Abilities[(int) AbilityIndex.Digestion];
            set => Abilities[(int) AbilityIndex.Digestion] = value;
        }

        /// <summary>反应力</summary>
        public UnboundedProperty Reaction
        {
            get => Abilities[(int) AbilityIndex.Reaction];
            set => Abilities[(int) AbilityIndex.Reaction] = value;
        }

        /// <summary>耐力</summary>
        public UnboundedProperty Endurance
        {
            get => Abilities[(int) AbilityIndex.Endurance];
            set => Abilities[(int) AbilityIndex.Endurance] = value;
        }

        /// <summary>毒素抗性</summary>
        public UnboundedProperty Antitoxic
        {
            get => Abilities[(int) AbilityIndex.Antitoxic];
            set => Abilities[(int) AbilityIndex.Antitoxic] = value;
        }

        /// <summary>上肢力量</summary>
        public UnboundedProperty UpperPower
        {
            get => Abilities[(int) AbilityIndex.UpperPower];
            set => Abilities[(int) AbilityIndex.UpperPower] = value;
        }

        /// <summary>下肢力量</summary>
        public UnboundedProperty LowerPower
        {
            get => Abilities[(int) AbilityIndex.LowerPower];
            set => Abilities[(int) AbilityIndex.LowerPower] = value;
        }

        /// <summary>上肢灵巧</summary>
        public UnboundedProperty UpperDexterity
        {
            get => Abilities[(int) AbilityIndex.UpperDexterity];
            set => Abilities[(int) AbilityIndex.UpperDexterity] = value;
        }

        /// <summary>下肢灵巧</summary>
        public UnboundedProperty LowerDexterity
        {
            get => Abilities[(int) AbilityIndex.LowerDexterity];
            set => Abilities[(int) AbilityIndex.LowerDexterity] = value;
        }

        #endregion

        #region Effect

        public UnboundedProperty[] Effects { get; set; } = new UnboundedProperty[EffectCount];

        public UnboundedProperty MaterialMagicPower
        {
            get => Effects[(int) EffectIndex.MaterialMagicPower];
            set => Effects[(int) EffectIndex.MaterialMagicPower] = value;
        }

        public UnboundedProperty DeformationMagicPower
        {
            get => Effects[(int) EffectIndex.DeformationMagicPower];
            set => Effects[(int) EffectIndex.DeformationMagicPower] = value;
        }

        public UnboundedProperty BuffMagicPower
        {
            get => Effects[(int) EffectIndex.BuffMagicPower];
            set => Effects[(int) EffectIndex.BuffMagicPower] = value;
        }

        public UnboundedProperty DecompositionMagicPower
        {
            get => Effects[(int) EffectIndex.DecompositionMagicPower];
            set => Effects[(int) EffectIndex.DecompositionMagicPower] = value;
        }

        public UnboundedProperty MagicLearningEfficiency
        {
            get => Effects[(int) EffectIndex.MagicLearningEfficiency];
            set => Effects[(int) EffectIndex.MagicLearningEfficiency] = value;
        }

        public UnboundedProperty SpiritResistance
        {
            get => Effects[(int) EffectIndex.SpiritResistance];
            set => Effects[(int) EffectIndex.SpiritResistance] = value;
        }

        public UnboundedProperty SummonMagicPower
        {
            get => Effects[(int) EffectIndex.SummonMagicPower];
            set => Effects[(int) EffectIndex.SummonMagicPower] = value;
        }

        public UnboundedProperty SpaceTimeMagicPower
        {
            get => Effects[(int) EffectIndex.SpaceTimeMagicPower];
            set => Effects[(int) EffectIndex.SpaceTimeMagicPower] = value;
        }

        public UnboundedProperty PerceptionMagicPower
        {
            get => Effects[(int) EffectIndex.PerceptionMagicPower];
            set => Effects[(int) EffectIndex.PerceptionMagicPower] = value;
        }

        public UnboundedProperty MagicTransformEfficiency
        {
            get => Effects[(int) EffectIndex.MagicTransformEfficiency];
            set => Effects[(int) EffectIndex.MagicTransformEfficiency] = value;
        }

        public UnboundedProperty ControllingMagicPower
        {
            get => Effects[(int) EffectIndex.ControllingMagicPower];
            set => Effects[(int) EffectIndex.ControllingMagicPower] = value;
        }

        public UnboundedProperty MagicHitRate
        {
            get => Effects[(int) EffectIndex.MagicHitRate];
            set => Effects[(int) EffectIndex.MagicHitRate] = value;
        }

        public UnboundedProperty MagicLearningRate
        {
            get => Effects[(int) EffectIndex.MagicLearningRate];
            set => Effects[(int) EffectIndex.MagicLearningRate] = value;
        }

        public UnboundedProperty AimingAccuracy
        {
            get => Effects[(int) EffectIndex.AimingAccuracy];
            set => Effects[(int) EffectIndex.AimingAccuracy] = value;
        }

        public UnboundedProperty SightRange
        {
            get => Effects[(int) EffectIndex.SightRange];
            set => Effects[(int) EffectIndex.SightRange] = value;
        }

        public UnboundedProperty InventionProbability
        {
            get => Effects[(int) EffectIndex.InventionProbability];
            set => Effects[(int) EffectIndex.InventionProbability] = value;
        }

        public UnboundedProperty AimingSpeed
        {
            get => Effects[(int) EffectIndex.AimingSpeed];
            set => Effects[(int) EffectIndex.AimingSpeed] = value;
        }

        public UnboundedProperty SkillLearningRate
        {
            get => Effects[(int) EffectIndex.SkillLearningRate];
            set => Effects[(int) EffectIndex.SkillLearningRate] = value;
        }

        public UnboundedProperty DigestionEfficiency
        {
            get => Effects[(int) EffectIndex.DigestionEfficiency];
            set => Effects[(int) EffectIndex.DigestionEfficiency] = value;
        }

        public UnboundedProperty MaximumEndurance
        {
            get => Effects[(int) EffectIndex.MaximumEndurance];
            set => Effects[(int) EffectIndex.MaximumEndurance] = value;
        }

        public UnboundedProperty TemperatureRange
        {
            get => Effects[(int) EffectIndex.TemperatureRange];
            set => Effects[(int) EffectIndex.TemperatureRange] = value;
        }

        public UnboundedProperty MaximumLoad
        {
            get => Effects[(int) EffectIndex.MaximumLoad];
            set => Effects[(int) EffectIndex.MaximumLoad] = value;
        }

        public UnboundedProperty UpperAttackEfficiency
        {
            get => Effects[(int) EffectIndex.UpperAttackEfficiency];
            set => Effects[(int) EffectIndex.UpperAttackEfficiency] = value;
        }

        public UnboundedProperty UpperAttackHitRate
        {
            get => Effects[(int) EffectIndex.UpperAttackHitRate];
            set => Effects[(int) EffectIndex.UpperAttackHitRate] = value;
        }

        public UnboundedProperty UpperAttackSpeed
        {
            get => Effects[(int) EffectIndex.UpperAttackSpeed];
            set => Effects[(int) EffectIndex.UpperAttackSpeed] = value;
        }

        public UnboundedProperty OperationEfficiency
        {
            get => Effects[(int) EffectIndex.OperationEfficiency];
            set => Effects[(int) EffectIndex.OperationEfficiency] = value;
        }

        public UnboundedProperty CraftEfficiency
        {
            get => Effects[(int) EffectIndex.CraftEfficiency];
            set => Effects[(int) EffectIndex.CraftEfficiency] = value;
        }

        public UnboundedProperty DodgeRate
        {
            get => Effects[(int) EffectIndex.DodgeRate];
            set => Effects[(int) EffectIndex.DodgeRate] = value;
        }

        public UnboundedProperty MaximumJumpHeight
        {
            get => Effects[(int) EffectIndex.MaximumJumpHeight];
            set => Effects[(int) EffectIndex.MaximumJumpHeight] = value;
        }

        public UnboundedProperty LowerAttackEfficiency
        {
            get => Effects[(int) EffectIndex.LowerAttackEfficiency];
            set => Effects[(int) EffectIndex.LowerAttackEfficiency] = value;
        }

        public UnboundedProperty LowerAttackHitRate
        {
            get => Effects[(int) EffectIndex.LowerAttackHitRate];
            set => Effects[(int) EffectIndex.LowerAttackHitRate] = value;
        }

        public UnboundedProperty LowerAttackSpeed
        {
            get => Effects[(int) EffectIndex.LowerAttackSpeed];
            set => Effects[(int) EffectIndex.LowerAttackSpeed] = value;
        }

        public UnboundedProperty MoveSpeed
        {
            get => Effects[(int) EffectIndex.MoveSpeed];
            set => Effects[(int) EffectIndex.MoveSpeed] = value;
        }

        #endregion
    }

    public class BodyPart
    {
        public uint        MaxHp        { get; set; }
        public uint        Hp           { get; set; }
        public uint        Potentiality { get; set; }
        public List<Wound> Wounds       { get; set; } = new List<Wound>();
        public float       Efficiency   => (float) Hp * Potentiality / MaxHp;

        public static BodyPart Create(uint potentiality, uint maxHp) => new BodyPart
        {
            Potentiality = potentiality,
            Hp           = maxHp,
            MaxHp        = maxHp
        };

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"Hp: {Hp}/{MaxHp} ({Efficiency}), P: {Potentiality}, W: {Wounds.Count}";
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

    public enum EffectIndex
    {
        /// <summary>
        /// 具现系魔法强度
        /// </summary>
        MaterialMagicPower,

        /// <summary>
        /// 变形系魔法强度
        /// </summary>
        DeformationMagicPower,

        /// <summary>
        /// 增强/衰减系魔法强度
        /// </summary>
        BuffMagicPower,

        /// <summary>
        /// 分解系魔法强度
        /// </summary>
        DecompositionMagicPower,

        /// <summary>
        /// 魔法学习效率
        /// </summary>
        MagicLearningEfficiency,

        /// <summary>
        /// 精神抗性
        /// </summary>
        SpiritResistance,

        /// <summary>
        /// 召唤系魔法强度
        /// </summary>
        SummonMagicPower,

        /// <summary>
        /// 时空系魔法强度
        /// </summary>
        SpaceTimeMagicPower,

        /// <summary>
        /// 感知系魔法强度
        /// </summary>
        PerceptionMagicPower,

        /// <summary>
        /// 魔力转换效率
        /// </summary>
        MagicTransformEfficiency,

        /// <summary>
        /// 控制系魔法强度
        /// </summary>
        ControllingMagicPower,

        /// <summary>
        /// 魔法命中
        /// </summary>
        MagicHitRate,

        /// <summary>
        /// 魔法学习速率
        /// </summary>
        MagicLearningRate,

        /// <summary>
        /// 瞄准精度
        /// </summary>
        AimingAccuracy,

        /// <summary>
        /// 视野距离
        /// </summary>
        SightRange,

        /// <summary>
        /// 发明基础概率
        /// </summary>
        InventionProbability,

        /// <summary>
        /// 瞄准速率
        /// </summary>
        AimingSpeed,

        /// <summary>
        /// 技能学习效率
        /// </summary>
        SkillLearningRate,

        /// <summary>
        /// 消化效率
        /// </summary>
        DigestionEfficiency,

        /// <summary>
        /// 最大体力
        /// </summary>
        MaximumEndurance,

        /// <summary>
        /// 适宜温度范围
        /// </summary>
        TemperatureRange,

        /// <summary>
        /// 负重
        /// </summary>
        MaximumLoad,

        /// <summary>
        /// 上肢攻击基础效率
        /// </summary>
        UpperAttackEfficiency,

        /// <summary>
        /// 上肢基础命中率
        /// </summary>
        UpperAttackHitRate,

        /// <summary>
        /// 上肢攻击基础速率
        /// </summary>
        UpperAttackSpeed,

        /// <summary>
        /// 操作效率
        /// </summary>
        OperationEfficiency,

        /// <summary>
        /// 制造效率
        /// </summary>
        CraftEfficiency,

        /// <summary>
        /// 基础闪避率
        /// </summary>
        DodgeRate,

        /// <summary>
        /// 最大跳跃高度
        /// </summary>
        MaximumJumpHeight,

        /// <summary>
        /// 下肢攻击基础效率
        /// </summary>
        LowerAttackEfficiency,

        /// <summary>
        /// 下肢基础命中率
        /// </summary>
        LowerAttackHitRate,

        /// <summary>
        /// 下肢攻击基础速率
        /// </summary>
        LowerAttackSpeed,

        /// <summary>
        /// 移动基础速率
        /// </summary>
        MoveSpeed,
        EffectCount
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
        MaxSlotCount
    }

    /// <summary xml:lang="cn">表示一种无最大值约束的属性，可以被施加线性和倍率加成</summary>
    public class UnboundedProperty
    {
        public float Base        { get; set; }
        public float LinearBonus { get; set; }
        public float ScaleBonus  { get; set; }

        public float Value => Base * (1 + ScaleBonus) + LinearBonus;

        /// <summary>Initializes a new instance of the <see cref="UnboundedProperty" /> struct.</summary>
        /// <param name="init">The initialize base.</param>
        public UnboundedProperty(float init)
        {
            Base = init;
        }

        /// <summary>Initializes a new instance of the <see cref="UnboundedProperty" /> struct.</summary>
        /// <param name="base">The base.</param>
        /// <param name="linearBonus">The linear bonus.</param>
        /// <param name="scaleBonus">The scale bonus.</param>
        public UnboundedProperty(float @base, float linearBonus, float scaleBonus)
        {
            Base        = @base;
            LinearBonus = linearBonus;
            ScaleBonus  = scaleBonus;
        }

        public UnboundedProperty() { }

        /// <summary>Performs an implicit conversion from <see cref="UnboundedProperty" /> to <see cref="float" />.</summary>
        /// <param name="property">The property.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float(UnboundedProperty property) => property.Value;

        /// <summary>Applies the linear bonus.</summary>
        /// <param name="linearBonus">The linear bonus.</param>
        public UnboundedProperty ApplyLinearBonus(float linearBonus)
        {
            LinearBonus += linearBonus;

            return this;
        }

        /// <summary>Applies the scale bonus.</summary>
        /// <param name="scaleBonus">The scale bonus.</param>
        public UnboundedProperty ApplyScaleBonus(float scaleBonus)
        {
            ScaleBonus += scaleBonus;

            return this;
        }

        public UnboundedProperty Update(float @base)
        {
            Base = @base;

            return this;
        }

        /// <summary>Resets the linear bonus.</summary>
        public UnboundedProperty ResetLinearBonus()
        {
            LinearBonus = 0;

            return this;
        }

        /// <summary>Resets the scale bonus.</summary>
        public UnboundedProperty ResetScaleBonus()
        {
            ScaleBonus = 0;

            return this;
        }

        /// <summary>Resets this instance.</summary>
        public UnboundedProperty Reset() => ResetLinearBonus().ResetScaleBonus();

        /// <summary>Returns the fully qualified type name of this instance.</summary>
        /// <returns>The fully qualified type name.</returns>
        public override string ToString() => $"{Value:N} ({Base:N} * {1 + ScaleBonus:N} + {LinearBonus:N})";
    }
}