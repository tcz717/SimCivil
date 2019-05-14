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
// SimCivil - SimCivil.Orleans.Grains - UnitGrain.cs
// Create Date: 2019/05/13
// Update Date: 2019/05/14

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;

namespace SimCivil.Orleans.Grains.Component
{
    [UsedImplicitly]
    public class UnitGrain : BaseGrain<UnitState>, IUnit
    {
        public UnitGrain(ILoggerFactory factory) : base(factory) { }

        public override Task<IReadOnlyDictionary<string, string>> Inspect(IEntity observer)
        {
            return Task.FromResult<IReadOnlyDictionary<string, string>>(
                typeof(UnitState).GetProperties().ToDictionary(p => p.Name, p => p.GetValue(State)?.ToString())
            );
        }

        /// <summary>Gets the heath point.</summary>
        /// <returns></returns>
        public Task<float> GetHp() => throw new NotImplementedException();

        public Task<UnitProperty> GetEffect(EffectIndex effectIndex)
            => Task.FromResult(State.Effects[(int) effectIndex]);

        public Task<UnitProperty> GetAbility(AbilityIndex abilityIndex)
            => Task.FromResult(State.Abilities[(int) abilityIndex]);

        public Task<BodyPart> GetBodyPart(BodyPartIndex bodyPartIndex)
            => Task.FromResult(State.BodyParts[(int) bodyPartIndex]);

        public Task UpdateAbilities()
        {
            UpdateReconstructionAbility();
            UpdateResolvingAbility();
            UpdatePerceptionAbility();
            UpdateControllingAbility();
            UpdateMentalAbility();
            UpdateCreativity();
            UpdateMemory();
            UpdateUnderstanding();
            UpdateConsciousness();
            UpdateDigestion();
            UpdateReaction();
            UpdateEndurance();
            UpdateAntitoxic();
            UpdateLowerDexterity();
            UpdateUpperDexterity();
            UpdateUpperPower();
            UpdateLowerPower();
            UpdateVision();

            return Task.CompletedTask;
        }

        public Task UpdateEffects()
        {
            UpdateMaterialMagicPower();
            UpdateDeformationMagicPower();
            UpdateBuffMagicPower();
            UpdateDecompositionMagicPower();
            UpdateMagicLearningEfficiency();
            UpdateSpiritResistance();
            UpdateSummonMagicPower();
            UpdateSpaceTimeMagicPower();
            UpdatePerceptionMagicPower();
            UpdateMagicTransformEfficiency();
            UpdateControllingMagicPower();
            UpdateMagicHitRate();
            UpdateMagicLearningRate();
            UpdateAimingAccuracy();
            UpdateSightRange();
            UpdateInventionProbability();
            UpdateAimingSpeed();
            UpdateSkillLearningRate();
            UpdateDigestionEfficiency();
            UpdateMaximumEndurance();
            UpdateTemperatureRange();
            UpdateMaximumLoad();
            UpdateUpperAttackEfficiency();
            UpdateUpperAttackHitRate();
            UpdateUpperAttackSpeed();
            UpdateOperationEfficiency();
            UpdateCraftEfficiency();
            UpdateDodgeRate();
            UpdateMaximumJumpHeight();
            UpdateLowerAttackEfficiency();
            UpdateLowerAttackHitRate();
            UpdateLowerAttackSpeed();
            UpdateMoveSpeed();

            return Task.CompletedTask;
        }

        private static T Min<T>(params T[] values) => values.Min();

        private static T Max<T>(params T[] values) => values.Max();

        #region AbilitiesUpdate

        public void UpdateReconstructionAbility() => State.ReconstructionAbility.Update(
            State.Soul.Efficiency);

        public void UpdateResolvingAbility() => State.ResolvingAbility.Update(State.Soul.Efficiency);

        public void UpdatePerceptionAbility() => State.PerceptionAbility.Update(State.Soul.Efficiency);

        public void UpdateControllingAbility() => State.ControllingAbility.Update(State.Soul.Efficiency);

        public void UpdateMentalAbility() => State.MentalAbility.Update(State.Soul.Efficiency);

        public void UpdateVision() => State.Vision = State.Vision.Update(
                                          Max(
                                              State.LeftEye.Efficiency,
                                              State.RightEye.Efficiency));

        public void UpdateCreativity() => State.Creativity.Update(State.Brain.Efficiency);

        public void UpdateMemory() => State.Memory.Update(State.Brain.Efficiency);

        public void UpdateUnderstanding() => State.Understanding.Update(State.Brain.Efficiency);

        public void UpdateConsciousness() => State.Consciousness.Update(State.Brain.Efficiency);

        public void UpdateDigestion() => State.Digestion.Update(
            Min(State.Digestive.Efficiency, State.Mouth.Efficiency));

        public void UpdateReaction() => State.Reaction.Update(State.Brain.Efficiency);

        public void UpdateEndurance() => State.Endurance.Update(
            Min(State.Heart.Efficiency, State.Lung.Efficiency));

        public void UpdateAntitoxic() => State.Antitoxic.Update(State.Immunity.Efficiency);

        public void UpdateUpperDexterity() => State.UpperDexterity.Update(
            Min(
                State.LeftArm.Efficiency,
                State.RightArm.Efficiency,
                State.LeftHand.Efficiency,
                State.RightHand.Efficiency));

        public void UpdateLowerDexterity() => State.LowerDexterity.Update(
            Min(
                State.LeftFoot.Efficiency,
                State.RightFoot.Efficiency,
                State.LeftLeg.Efficiency,
                State.RightLeg.Efficiency));

        public void UpdateUpperPower() => State.UpperPower.Update(
            Min(
                State.LeftArm.Efficiency,
                State.RightArm.Efficiency,
                State.LeftHand.Efficiency,
                State.RightHand.Efficiency));

        public void UpdateLowerPower() => State.LowerPower.Update(
            Min(
                State.LeftFoot.Efficiency,
                State.RightFoot.Efficiency,
                State.LeftLeg.Efficiency,
                State.RightLeg.Efficiency));

        #endregion

        #region EffectsUpdate

        // TODO: MentalAbility and Consciousness should influence all effects
        public void UpdateMaterialMagicPower() => State.MaterialMagicPower.Update(State.ReconstructionAbility);

        public void UpdateDeformationMagicPower()
            => State.DeformationMagicPower.Update(Min(State.ReconstructionAbility, State.ResolvingAbility));

        public void UpdateBuffMagicPower()
            => State.BuffMagicPower.Update(Min(State.ReconstructionAbility, State.ResolvingAbility));

        public void UpdateDecompositionMagicPower()
            => State.DecompositionMagicPower.Update(State.ResolvingAbility);

        public void UpdateMagicLearningEfficiency()
            => State.MagicLearningEfficiency.Update(Min(State.PerceptionAbility, State.ResolvingAbility));

        public void UpdateSpiritResistance()
            => State.SpiritResistance.Update(Min(State.PerceptionAbility, State.ResolvingAbility));

        public void UpdateSummonMagicPower()
            => State.SummonMagicPower.Update(Min(State.ReconstructionAbility, State.ControllingAbility));

        public void UpdateSpaceTimeMagicPower()
            => State.SpaceTimeMagicPower.Update(
                Min(
                    State.ReconstructionAbility,
                    State.ControllingAbility,
                    State.PerceptionAbility,
                    State.ResolvingAbility));

        public void UpdatePerceptionMagicPower() => State.PerceptionMagicPower.Update(State.PerceptionAbility);

        public void UpdateMagicTransformEfficiency()
            => State.MagicTransformEfficiency.Update(Min(State.PerceptionAbility, State.ControllingAbility));

        public void UpdateControllingMagicPower() => State.ControllingMagicPower.Update(State.ControllingAbility);
        public void UpdateMagicHitRate()          => State.MagicHitRate.Update(State.ControllingAbility);
        public void UpdateMagicLearningRate()     => State.MagicLearningRate.Update(State.ControllingAbility);
        public void UpdateAimingAccuracy()        => State.AimingAccuracy.Update(State.Vision);
        public void UpdateSightRange()            => State.SightRange.Update(State.Vision);
        public void UpdateInventionProbability()  => State.InventionProbability.Update(State.InventionProbability);
        public void UpdateAimingSpeed()           => State.AimingSpeed.Update(Min(State.Vision, State.Reaction));

        public void UpdateSkillLearningRate()
            => State.SkillLearningRate.Update(Min(State.Memory, State.Understanding));

        public void UpdateDigestionEfficiency()   => State.DigestionEfficiency.Update(State.Digestion);
        public void UpdateMaximumEndurance()      => State.MaximumEndurance.Update(State.Endurance);
        public void UpdateTemperatureRange()      => State.TemperatureRange.Update(State.Endurance);
        public void UpdateMaximumLoad()           => State.MaximumLoad.Update(State.Endurance);
        public void UpdateUpperAttackEfficiency() => State.UpperAttackEfficiency.Update(State.UpperPower);

        public void UpdateUpperAttackHitRate()
            => State.UpperAttackHitRate.Update(Min(State.Vision, State.Reaction, State.UpperDexterity));

        public void UpdateUpperAttackSpeed() => State.UpperAttackSpeed.Update(State.UpperDexterity);

        public void UpdateOperationEfficiency()
            => State.OperationEfficiency.Update(Min(State.UpperDexterity, State.UpperPower));

        public void UpdateCraftEfficiency() => State.CraftEfficiency.Update(State.UpperDexterity);

        public void UpdateDodgeRate()
            => State.DodgeRate.Update(Min(State.Reaction, State.UpperDexterity, State.LowerDexterity));

        public void UpdateMaximumJumpHeight()     => State.MaximumJumpHeight.Update(State.LowerPower);
        public void UpdateLowerAttackEfficiency() => State.LowerAttackEfficiency.Update(State.LowerPower);

        public void UpdateLowerAttackHitRate()
            => State.LowerAttackHitRate.Update(Min(State.Vision, State.Reaction, State.LowerDexterity));

        public void UpdateLowerAttackSpeed() => State.LowerAttackSpeed.Update(State.LowerDexterity);
        public void UpdateMoveSpeed()        => State.MoveSpeed.Update(State.LowerDexterity);

        #endregion
    }
}