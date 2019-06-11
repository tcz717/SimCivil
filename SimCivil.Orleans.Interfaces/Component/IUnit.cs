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
// SimCivil - SimCivil.Orleans.Interfaces - IUnit.cs
// Create Date: 2019/05/31
// Update Date: 2019/05/31

using System;
using System.Text;
using System.Threading.Tasks;

using SimCivil.Orleans.Interfaces.Component.State;

namespace SimCivil.Orleans.Interfaces.Component
{
    public interface IUnit : IComponent<UnitState>
    {
        /// <summary>Gets the hp.</summary>
        /// <returns></returns>
        Task<float> GetHp();

        /// <summary>Updates the abilities.</summary>
        /// <returns></returns>
        Task UpdateAbilities();

        /// <summary>Updates the effects.</summary>
        /// <returns></returns>
        Task UpdateEffects();

        Task<UnitProperty> GetEffect(EffectIndex     effectIndex);
        Task<UnitProperty> GetAbility(AbilityIndex   abilityIndex);
        Task<BodyPart>     GetBodyPart(BodyPartIndex bodyPartIndex);
    }

    public static class UnitExtension
    {
        public static Task<float> GetMoveSpeed(this IUnit unit)
        {
            return unit.GetEffect(EffectIndex.MoveSpeed).ContinueWith(p => p.Result.Value);
        }

        public static Task<uint> GetSightRange(this IUnit unit)
        {
            return unit.GetEffect(EffectIndex.SightRange).ContinueWith(p => (uint) p.Result.Value);
        }
    }
}