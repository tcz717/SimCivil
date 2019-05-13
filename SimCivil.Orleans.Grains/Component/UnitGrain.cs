﻿// Copyright (c) 2017 TPDT
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
// Create Date: 2018/12/13
// Update Date: 2018/12/30

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;

namespace SimCivil.Orleans.Grains.Component
{
    [UsedImplicitly]
    public class UnitGrain : BaseGrain<UnitState>, IUnit
    {
        public UnitGrain(ILoggerFactory factory) : base(factory) { }

        public Task Fill(CreateRoleOption option)
        {
            State.Gender = option.Gender;

            return Task.CompletedTask;
        }

        public override Task<IReadOnlyDictionary<string, string>> Inspect(IEntity observer)
        {
            return Task.FromResult<IReadOnlyDictionary<string, string>>(
                typeof(UnitState).GetProperties().ToDictionary(p => p.Name, p => p.GetValue(State)?.ToString())
            );
        }

        /// <summary>Gets the heath point.</summary>
        /// <returns></returns>
        public Task<float> GetHp() => throw new NotImplementedException();

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
            UpdateMoveSpeed();
            UpdateSightRange();

            return Task.CompletedTask;
        }

        public Task<float> GetMoveSpeed() => Task.FromResult(State.MoveSpeed.Value);

        public Task<uint> GetSightRange() => Task.FromResult((uint) State.SightRange.Value);

        private static T Min<T>(params T[] values) => values.Min();

        private static T Max<T>(params T[] values) => values.Max();

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


        public void UpdateSightRange() => State.SightRange = State.SightRange.Update(State.Vision);

        public void UpdateMoveSpeed() => State.MoveSpeed = State.MoveSpeed.Update(State.LowerDexterity);
    }
}