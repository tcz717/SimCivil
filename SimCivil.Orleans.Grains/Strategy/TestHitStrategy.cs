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
// SimCivil - SimCivil.Orleans.Grains - TestHitStrategy.cs
// Create Date: 2019/06/06
// Update Date: 2019/06/06

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Orleans;

using SimCivil.Contract.Model;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Strategy;
using SimCivil.Utilities.AutoService;

namespace SimCivil.Orleans.Grains.Strategy
{
    [PublicAPI]
    [AutoService(ServiceLifetime.Transient)]
    public class TestHitStrategy : IHitStrategy
    {
        public TestHitStrategy(IGrainFactory grainFactory, IOptions<HitStrategyOptions> options)
        {
            GrainFactory = grainFactory;
            Options      = options.Value;
        }

        public Random             Rand         { get; } = new Random();
        public IGrainFactory      GrainFactory { get; }
        public HitStrategyOptions Options      { get; }

        public async Task<ImmutableDictionary<BodyPartIndex, Wound>> HitCalculateAsync(
            IEntity   attacker,
            IEntity   defender,
            IEntity   injurant,
            HitMethod hitMethod)
        {
            switch (hitMethod)
            {
                case HitMethod.Fist:
                case HitMethod.Foot:
                {
                    if (injurant == null)
                        return await HitCalculateWithoutInjurantAsync(attacker, defender, hitMethod);

                    throw new NotSupportedException();
                }

                case HitMethod.Melee:
                case HitMethod.Magic:
                case HitMethod.Projectile:
                case HitMethod.Temperature:
                case HitMethod.Trap:
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException(nameof(hitMethod), hitMethod, null);
            }
        }

        private async Task<ImmutableDictionary<BodyPartIndex, Wound>> HitCalculateWithoutInjurantAsync(
            IEntity   attacker,
            IEntity   defender,
            HitMethod hitMethod)
        {
            UnitState attackerState = await GrainFactory.Get<IUnit>(attacker).GetData();
            UnitState defenderState = await GrainFactory.Get<IUnit>(defender).GetData();

            UnitProperty hitRate;
            UnitProperty dodgeRate = defenderState.DodgeRate;
            UnitProperty attackEfficiency;

            switch (hitMethod)
            {
                case HitMethod.Fist:
                    hitRate          = attackerState.UpperAttackHitRate;
                    attackEfficiency = attackerState.UpperAttackEfficiency;

                    break;
                case HitMethod.Foot:
                    hitRate          = attackerState.LowerAttackHitRate;
                    attackEfficiency = attackerState.LowerAttackEfficiency;

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(hitMethod), hitMethod, null);
            }

            double hitP = Math.Atan(hitRate / dodgeRate) * 2 / Math.PI;

            if (Rand.NextDouble() > hitP) return ImmutableDictionary<BodyPartIndex, Wound>.Empty;

            var attackResult = new Dictionary<BodyPartIndex, Wound>();

            for (var i = 0; i < (int) BodyPartIndex.BodyPartCount; i++)
                if (Options.BodyHitBaseProbability[i] > Rand.NextDouble())
                    attackResult.Add((BodyPartIndex) i, new Wound((uint) attackEfficiency, WoundType.Bruise));

            return attackResult.ToImmutableDictionary();
        }
    }
}