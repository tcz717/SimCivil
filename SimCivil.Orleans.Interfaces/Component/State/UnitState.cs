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
// SimCivil - SimCivil.Orleans.Interfaces - UnitState.cs
// Create Date: 2019/05/13
// Update Date: 2019/05/28

using System;
using System.Collections.Generic;
using System.Text;

using SimCivil.Contract.Model;

namespace SimCivil.Orleans.Interfaces.Component.State
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

        public UnitProperty[] Abilities { get; set; } = new UnitProperty[AbilityCount];

        /// <summary>重构能力</summary>
        public UnitProperty ReconstructionAbility
        {
            get => Abilities[(int) AbilityIndex.ReconstructionAbility];
            set => Abilities[(int) AbilityIndex.ReconstructionAbility] = value;
        }

        /// <summary>解析能力</summary>
        public UnitProperty ResolvingAbility
        {
            get => Abilities[(int) AbilityIndex.ResolvingAbility];
            set => Abilities[(int) AbilityIndex.ResolvingAbility] = value;
        }

        /// <summary>感知能力</summary>
        public UnitProperty PerceptionAbility
        {
            get => Abilities[(int) AbilityIndex.PerceptionAbility];
            set => Abilities[(int) AbilityIndex.PerceptionAbility] = value;
        }

        /// <summary>控制能力</summary>
        public UnitProperty ControllingAbility
        {
            get => Abilities[(int) AbilityIndex.ControllingAbility];
            set => Abilities[(int) AbilityIndex.ControllingAbility] = value;
        }

        /// <summary>精神力</summary>
        public UnitProperty MentalAbility
        {
            get => Abilities[(int) AbilityIndex.MentalAbility];
            set => Abilities[(int) AbilityIndex.MentalAbility] = value;
        }

        /// <summary>视力</summary>
        public UnitProperty Vision
        {
            get => Abilities[(int) AbilityIndex.Vision];
            set => Abilities[(int) AbilityIndex.Vision] = value;
        }

        /// <summary>创造力</summary>
        public UnitProperty Creativity
        {
            get => Abilities[(int) AbilityIndex.Creativity];
            set => Abilities[(int) AbilityIndex.Creativity] = value;
        }

        /// <summary>记忆力</summary>
        public UnitProperty Memory
        {
            get => Abilities[(int) AbilityIndex.Memory];
            set => Abilities[(int) AbilityIndex.Memory] = value;
        }

        /// <summary>理解力</summary>
        public UnitProperty Understanding
        {
            get => Abilities[(int) AbilityIndex.Understanding];
            set => Abilities[(int) AbilityIndex.Understanding] = value;
        }

        /// <summary>意识</summary>
        public UnitProperty Consciousness
        {
            get => Abilities[(int) AbilityIndex.Consciousness];
            set => Abilities[(int) AbilityIndex.Consciousness] = value;
        }

        /// <summary>消化能力</summary>
        public UnitProperty Digestion
        {
            get => Abilities[(int) AbilityIndex.Digestion];
            set => Abilities[(int) AbilityIndex.Digestion] = value;
        }

        /// <summary>反应力</summary>
        public UnitProperty Reaction
        {
            get => Abilities[(int) AbilityIndex.Reaction];
            set => Abilities[(int) AbilityIndex.Reaction] = value;
        }

        /// <summary>耐力</summary>
        public UnitProperty Endurance
        {
            get => Abilities[(int) AbilityIndex.Endurance];
            set => Abilities[(int) AbilityIndex.Endurance] = value;
        }

        /// <summary>毒素抗性</summary>
        public UnitProperty Antitoxic
        {
            get => Abilities[(int) AbilityIndex.Antitoxic];
            set => Abilities[(int) AbilityIndex.Antitoxic] = value;
        }

        /// <summary>上肢力量</summary>
        public UnitProperty UpperPower
        {
            get => Abilities[(int) AbilityIndex.UpperPower];
            set => Abilities[(int) AbilityIndex.UpperPower] = value;
        }

        /// <summary>下肢力量</summary>
        public UnitProperty LowerPower
        {
            get => Abilities[(int) AbilityIndex.LowerPower];
            set => Abilities[(int) AbilityIndex.LowerPower] = value;
        }

        /// <summary>上肢灵巧</summary>
        public UnitProperty UpperDexterity
        {
            get => Abilities[(int) AbilityIndex.UpperDexterity];
            set => Abilities[(int) AbilityIndex.UpperDexterity] = value;
        }

        /// <summary>下肢灵巧</summary>
        public UnitProperty LowerDexterity
        {
            get => Abilities[(int) AbilityIndex.LowerDexterity];
            set => Abilities[(int) AbilityIndex.LowerDexterity] = value;
        }

        #endregion

        #region Effect

        public UnitProperty[] Effects { get; set; } = new UnitProperty[EffectCount];

        public UnitProperty MaterialMagicPower
        {
            get => Effects[(int) EffectIndex.MaterialMagicPower];
            set => Effects[(int) EffectIndex.MaterialMagicPower] = value;
        }

        public UnitProperty DeformationMagicPower
        {
            get => Effects[(int) EffectIndex.DeformationMagicPower];
            set => Effects[(int) EffectIndex.DeformationMagicPower] = value;
        }

        public UnitProperty BuffMagicPower
        {
            get => Effects[(int) EffectIndex.BuffMagicPower];
            set => Effects[(int) EffectIndex.BuffMagicPower] = value;
        }

        public UnitProperty DecompositionMagicPower
        {
            get => Effects[(int) EffectIndex.DecompositionMagicPower];
            set => Effects[(int) EffectIndex.DecompositionMagicPower] = value;
        }

        public UnitProperty MagicLearningEfficiency
        {
            get => Effects[(int) EffectIndex.MagicLearningEfficiency];
            set => Effects[(int) EffectIndex.MagicLearningEfficiency] = value;
        }

        public UnitProperty SpiritResistance
        {
            get => Effects[(int) EffectIndex.SpiritResistance];
            set => Effects[(int) EffectIndex.SpiritResistance] = value;
        }

        public UnitProperty SummonMagicPower
        {
            get => Effects[(int) EffectIndex.SummonMagicPower];
            set => Effects[(int) EffectIndex.SummonMagicPower] = value;
        }

        public UnitProperty SpaceTimeMagicPower
        {
            get => Effects[(int) EffectIndex.SpaceTimeMagicPower];
            set => Effects[(int) EffectIndex.SpaceTimeMagicPower] = value;
        }

        public UnitProperty PerceptionMagicPower
        {
            get => Effects[(int) EffectIndex.PerceptionMagicPower];
            set => Effects[(int) EffectIndex.PerceptionMagicPower] = value;
        }

        public UnitProperty MagicTransformEfficiency
        {
            get => Effects[(int) EffectIndex.MagicTransformEfficiency];
            set => Effects[(int) EffectIndex.MagicTransformEfficiency] = value;
        }

        public UnitProperty ControllingMagicPower
        {
            get => Effects[(int) EffectIndex.ControllingMagicPower];
            set => Effects[(int) EffectIndex.ControllingMagicPower] = value;
        }

        public UnitProperty MagicHitRate
        {
            get => Effects[(int) EffectIndex.MagicHitRate];
            set => Effects[(int) EffectIndex.MagicHitRate] = value;
        }

        public UnitProperty MagicLearningRate
        {
            get => Effects[(int) EffectIndex.MagicLearningRate];
            set => Effects[(int) EffectIndex.MagicLearningRate] = value;
        }

        public UnitProperty AimingAccuracy
        {
            get => Effects[(int) EffectIndex.AimingAccuracy];
            set => Effects[(int) EffectIndex.AimingAccuracy] = value;
        }

        public UnitProperty SightRange
        {
            get => Effects[(int) EffectIndex.SightRange];
            set => Effects[(int) EffectIndex.SightRange] = value;
        }

        public UnitProperty InventionProbability
        {
            get => Effects[(int) EffectIndex.InventionProbability];
            set => Effects[(int) EffectIndex.InventionProbability] = value;
        }

        public UnitProperty AimingSpeed
        {
            get => Effects[(int) EffectIndex.AimingSpeed];
            set => Effects[(int) EffectIndex.AimingSpeed] = value;
        }

        public UnitProperty SkillLearningRate
        {
            get => Effects[(int) EffectIndex.SkillLearningRate];
            set => Effects[(int) EffectIndex.SkillLearningRate] = value;
        }

        public UnitProperty DigestionEfficiency
        {
            get => Effects[(int) EffectIndex.DigestionEfficiency];
            set => Effects[(int) EffectIndex.DigestionEfficiency] = value;
        }

        public UnitProperty MaximumEndurance
        {
            get => Effects[(int) EffectIndex.MaximumEndurance];
            set => Effects[(int) EffectIndex.MaximumEndurance] = value;
        }

        public UnitProperty TemperatureRange
        {
            get => Effects[(int) EffectIndex.TemperatureRange];
            set => Effects[(int) EffectIndex.TemperatureRange] = value;
        }

        public UnitProperty MaximumLoad
        {
            get => Effects[(int) EffectIndex.MaximumLoad];
            set => Effects[(int) EffectIndex.MaximumLoad] = value;
        }

        public UnitProperty UpperAttackEfficiency
        {
            get => Effects[(int) EffectIndex.UpperAttackEfficiency];
            set => Effects[(int) EffectIndex.UpperAttackEfficiency] = value;
        }

        public UnitProperty UpperAttackHitRate
        {
            get => Effects[(int) EffectIndex.UpperAttackHitRate];
            set => Effects[(int) EffectIndex.UpperAttackHitRate] = value;
        }

        public UnitProperty UpperAttackSpeed
        {
            get => Effects[(int) EffectIndex.UpperAttackSpeed];
            set => Effects[(int) EffectIndex.UpperAttackSpeed] = value;
        }

        public UnitProperty OperationEfficiency
        {
            get => Effects[(int) EffectIndex.OperationEfficiency];
            set => Effects[(int) EffectIndex.OperationEfficiency] = value;
        }

        public UnitProperty CraftEfficiency
        {
            get => Effects[(int) EffectIndex.CraftEfficiency];
            set => Effects[(int) EffectIndex.CraftEfficiency] = value;
        }

        public UnitProperty DodgeRate
        {
            get => Effects[(int) EffectIndex.DodgeRate];
            set => Effects[(int) EffectIndex.DodgeRate] = value;
        }

        public UnitProperty MaximumJumpHeight
        {
            get => Effects[(int) EffectIndex.MaximumJumpHeight];
            set => Effects[(int) EffectIndex.MaximumJumpHeight] = value;
        }

        public UnitProperty LowerAttackEfficiency
        {
            get => Effects[(int) EffectIndex.LowerAttackEfficiency];
            set => Effects[(int) EffectIndex.LowerAttackEfficiency] = value;
        }

        public UnitProperty LowerAttackHitRate
        {
            get => Effects[(int) EffectIndex.LowerAttackHitRate];
            set => Effects[(int) EffectIndex.LowerAttackHitRate] = value;
        }

        public UnitProperty LowerAttackSpeed
        {
            get => Effects[(int) EffectIndex.LowerAttackSpeed];
            set => Effects[(int) EffectIndex.LowerAttackSpeed] = value;
        }

        public UnitProperty MoveSpeed
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

    public class Wound
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object"></see> class.</summary>
        public Wound(uint damage, WoundType type, bool permanent = false)
        {
            Damage    = damage;
            Type      = type;
            Permanent = permanent;
        }

        public uint      Damage    { get; set; }
        public WoundType Type      { get; set; }
        public bool      Permanent { get; set; }
    }

    public enum WoundType
    {
        /// <summary>
        /// 瘀伤
        /// </summary>
        Bruise
    }
}