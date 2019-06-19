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
// SimCivil - SimCivil.Orleans.Grains - SimpleDamageService.cs
// Create Date: 2019/06/14
// Update Date: 2019/06/16

using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;

using SimCivil.Contract.Model;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;
using SimCivil.Orleans.Interfaces.Strategy;
using SimCivil.Utilities.AutoService;

namespace SimCivil.Orleans.Grains.Service
{
    [AutoService(ServiceLifetime.Singleton)]
    public class SimpleDamageService : IDamageService
    {
        public           ILogger<SimpleDamageService>      Logger { get; }
        private readonly IMapService                       _map;
        private readonly IHitStrategy                      _hitStrategy;
        private readonly IOptions<DamageOptions>           _options;
        private readonly IGrainFactory                     _grainFactory;
        private readonly ReadOnlyCollection<BodyPartIndex> _deadlyParts;

        public SimpleDamageService(
            IMapService                  map,
            IHitStrategy                 hitStrategy,
            ILogger<SimpleDamageService> logger,
            IOptions<DamageOptions>      options,
            IGrainFactory                grainFactory)
        {
            Logger        = logger;
            _map          = map;
            _hitStrategy  = hitStrategy;
            _options      = options;
            _grainFactory = grainFactory;
            _deadlyParts  = new ReadOnlyCollection<BodyPartIndex>(options.Value.DeadlyBodyParts);
        }

        public async Task<AttackResult> Attack(
            IEntity   attacker,
            IEntity   defender,
            IEntity   injurant,
            HitMethod hitMethod)
        {
            float d = await _map.CalculateDistance(attacker, defender);
            ImmutableDictionary<BodyPartIndex, Wound> wounds;

            UnitProperty speed;
            switch (hitMethod)
            {
                case HitMethod.Fist:
                    if (d > _options.Value.UpperBaseAttackRange) return AttackResult.OutOfRange();

                    speed = await _grainFactory.Get<IUnit>(attacker).GetEffect(EffectIndex.UpperAttackSpeed);

                    if (!await _grainFactory.Get<ICooldown>(attacker)
                                            .TryDo("UpperAttack", TimeSpan.FromSeconds(1 / speed)))
                        return AttackResult.Cooldown();

                    wounds = await CloseAttack(attacker, defender, hitMethod);

                    break;
                case HitMethod.Foot:
                    if (d > _options.Value.LowerBaseAttackRange) return AttackResult.OutOfRange();
                    speed = await _grainFactory.Get<IUnit>(attacker).GetEffect(EffectIndex.LowerAttackSpeed);

                    if (!await _grainFactory.Get<ICooldown>(attacker)
                                            .TryDo("LowerAttack", TimeSpan.FromSeconds(1 / speed)))
                        return AttackResult.Cooldown();

                    wounds = await CloseAttack(attacker, defender, hitMethod);

                    break;
                case HitMethod.Melee:       return AttackResult.OutOfRange();
                case HitMethod.Magic:       return AttackResult.OutOfRange();
                case HitMethod.Projectile:  return AttackResult.OutOfRange();
                case HitMethod.Temperature: return AttackResult.OutOfRange();
                case HitMethod.Trap:        return AttackResult.OutOfRange();
                default:
                    throw new ArgumentOutOfRangeException(nameof(hitMethod), hitMethod, null);
            }

            var defenderUnit = _grainFactory.Get<IUnit>(defender);
            UnitState unitState = await defenderUnit.ApplyWounds(wounds);

            for (var i = 0; i < unitState.BodyParts.Length; i++)
            {
                var bodyPartIndex = (BodyPartIndex) i;

                if (unitState.BodyParts[i].Hp != 0 || !_deadlyParts.Contains(bodyPartIndex)) continue;

                await defenderUnit.MarkDead(attacker);

                return AttackResult.Dead(wounds);
            }

            return AttackResult.Hit(wounds);
        }

        private async Task<ImmutableDictionary<BodyPartIndex, Wound>> CloseAttack(
            IEntity   attacker,
            IEntity   defender,
            HitMethod hitMethod) => await _hitStrategy.HitCalculateAsync(attacker, defender, null, hitMethod);
    }
}