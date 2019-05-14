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

        private void UpdateReconstructionAbility() => State.ReconstructionAbility.Update(
            State.Soul.Efficiency);

        private void UpdateResolvingAbility() => State.ResolvingAbility.Update(State.Soul.Efficiency);

        private void UpdatePerceptionAbility() => State.PerceptionAbility.Update(State.Soul.Efficiency);

        private void UpdateControllingAbility() => State.ControllingAbility.Update(State.Soul.Efficiency);

        private void UpdateMentalAbility() => State.MentalAbility.Update(State.Soul.Efficiency);

        private void UpdateVision() => State.Vision = State.Vision.Update(
                                          Max(
                                              State.LeftEye.Efficiency,
                                              State.RightEye.Efficiency));

        private void UpdateCreativity() => State.Creativity.Update(State.Brain.Efficiency);

        private void UpdateMemory() => State.Memory.Update(State.Brain.Efficiency);

        private void UpdateUnderstanding() => State.Understanding.Update(State.Brain.Efficiency);

        private void UpdateConsciousness() => State.Consciousness.Update(State.Brain.Efficiency);

        private void UpdateDigestion() => State.Digestion.Update(
            Min(State.Digestive.Efficiency, State.Mouth.Efficiency));

        private void UpdateReaction() => State.Reaction.Update(State.Brain.Efficiency);

        private void UpdateEndurance() => State.Endurance.Update(
            Min(State.Heart.Efficiency, State.Lung.Efficiency));

        private void UpdateAntitoxic() => State.Antitoxic.Update(State.Immunity.Efficiency);

        private void UpdateUpperDexterity() => State.UpperDexterity.Update(
            Min(
                State.LeftArm.Efficiency,
                State.RightArm.Efficiency,
                State.LeftHand.Efficiency,
                State.RightHand.Efficiency));

        private void UpdateLowerDexterity() => State.LowerDexterity.Update(
            Min(
                State.LeftFoot.Efficiency,
                State.RightFoot.Efficiency,
                State.LeftLeg.Efficiency,
                State.RightLeg.Efficiency));

        private void UpdateUpperPower() => State.UpperPower.Update(
            Min(
                State.LeftArm.Efficiency,
                State.RightArm.Efficiency,
                State.LeftHand.Efficiency,
                State.RightHand.Efficiency));

        private void UpdateLowerPower() => State.LowerPower.Update(
            Min(
                State.LeftFoot.Efficiency,
                State.RightFoot.Efficiency,
                State.LeftLeg.Efficiency,
                State.RightLeg.Efficiency));

        #endregion

        #region EffectsUpdate

        // TODO: MentalAbility and Consciousness should influence all effects
        private void UpdateMaterialMagicPower() => State.MaterialMagicPower.Update(State.ReconstructionAbility);

        private void UpdateDeformationMagicPower()
            => State.DeformationMagicPower.Update(Min(State.ReconstructionAbility, State.ResolvingAbility));

        private void UpdateBuffMagicPower()
            => State.BuffMagicPower.Update(Min(State.ReconstructionAbility, State.ResolvingAbility));

        private void UpdateDecompositionMagicPower()
            => State.DecompositionMagicPower.Update(State.ResolvingAbility);

        private void UpdateMagicLearningEfficiency()
            => State.MagicLearningEfficiency.Update(Min(State.PerceptionAbility, State.ResolvingAbility));

        private void UpdateSpiritResistance()
            => State.SpiritResistance.Update(Min(State.PerceptionAbility, State.ResolvingAbility));

        private void UpdateSummonMagicPower()
            => State.SummonMagicPower.Update(Min(State.ReconstructionAbility, State.ControllingAbility));

        private void UpdateSpaceTimeMagicPower()
            => State.SpaceTimeMagicPower.Update(
                Min(
                    State.ReconstructionAbility,
                    State.ControllingAbility,
                    State.PerceptionAbility,
                    State.ResolvingAbility));

        private void UpdatePerceptionMagicPower() => State.PerceptionMagicPower.Update(State.PerceptionAbility);

        private void UpdateMagicTransformEfficiency()
            => State.MagicTransformEfficiency.Update(Min(State.PerceptionAbility, State.ControllingAbility));

        private void UpdateControllingMagicPower() => State.ControllingMagicPower.Update(State.ControllingAbility);
        private void UpdateMagicHitRate()          => State.MagicHitRate.Update(State.ControllingAbility);
        private void UpdateMagicLearningRate()     => State.MagicLearningRate.Update(State.ControllingAbility);
        private void UpdateAimingAccuracy()        => State.AimingAccuracy.Update(State.Vision);
        private void UpdateSightRange()            => State.SightRange.Update(State.Vision);
        private void UpdateInventionProbability()  => State.InventionProbability.Update(State.InventionProbability);
        private void UpdateAimingSpeed()           => State.AimingSpeed.Update(Min(State.Vision, State.Reaction));

        private void UpdateSkillLearningRate()
            => State.SkillLearningRate.Update(Min(State.Memory, State.Understanding));

        private void UpdateDigestionEfficiency()   => State.DigestionEfficiency.Update(State.Digestion);
        private void UpdateMaximumEndurance()      => State.MaximumEndurance.Update(State.Endurance);
        private void UpdateTemperatureRange()      => State.TemperatureRange.Update(State.Endurance);
        private void UpdateMaximumLoad()           => State.MaximumLoad.Update(State.Endurance);
        private void UpdateUpperAttackEfficiency() => State.UpperAttackEfficiency.Update(State.UpperPower);

        private void UpdateUpperAttackHitRate()
            => State.UpperAttackHitRate.Update(Min(State.Vision, State.Reaction, State.UpperDexterity));

        private void UpdateUpperAttackSpeed() => State.UpperAttackSpeed.Update(State.UpperDexterity);

        private void UpdateOperationEfficiency()
            => State.OperationEfficiency.Update(Min(State.UpperDexterity, State.UpperPower));

        private void UpdateCraftEfficiency() => State.CraftEfficiency.Update(State.UpperDexterity);

        private void UpdateDodgeRate()
            => State.DodgeRate.Update(Min(State.Reaction, State.UpperDexterity, State.LowerDexterity));

        private void UpdateMaximumJumpHeight()     => State.MaximumJumpHeight.Update(State.LowerPower);
        private void UpdateLowerAttackEfficiency() => State.LowerAttackEfficiency.Update(State.LowerPower);

        private void UpdateLowerAttackHitRate()
            => State.LowerAttackHitRate.Update(Min(State.Vision, State.Reaction, State.LowerDexterity));

        private void UpdateLowerAttackSpeed() => State.LowerAttackSpeed.Update(State.LowerDexterity);
        private void UpdateMoveSpeed()        => State.MoveSpeed.Update(State.LowerDexterity);

        #endregion
    }
}