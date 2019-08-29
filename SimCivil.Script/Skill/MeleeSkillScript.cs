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
// SimCivil - SimCivil.Script - MeleeSkillScript.cs
// Create Date: 2019/08/29
// Update Date: 2019/08/29

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;

using SimCivil.Contract.Model;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using SimCivil.Orleans.Interfaces.Option;
using SimCivil.Orleans.Interfaces.Service;
using SimCivil.Orleans.Interfaces.Skill;

namespace SimCivil.Script.Skill
{
    public class MeleeSkillScript : ISkillScript
    {
        public const string SkillRange    = "SkillRange";
        public const string CoolDownUpper = "Upper";
        public const string CoolDown      = "CoolDown";

        private readonly IMapService                       _map;
        private          ILogger<MeleeSkillScript>         _logger;
        private readonly IOptions<BattleOptions>           _options;
        private readonly IGrainFactory                     _grainFactory;
        private readonly ICoolDownService                  _coolDown;
        private readonly ReadOnlyCollection<BodyPartIndex> _deadlyParts;
        public           Random                            Rand { get; } = new Random();

        public string Name { get; } = "Melee";


        public MeleeSkillScript(
            IMapService               map,
            ILogger<MeleeSkillScript> logger,
            IOptions<BattleOptions>   options,
            IGrainFactory             grainFactory,
            ICoolDownService          coolDown)
        {
            _logger       = logger;
            _map          = map;
            _options      = options;
            _grainFactory = grainFactory;
            _coolDown     = coolDown;
            _deadlyParts  = new ReadOnlyCollection<BodyPartIndex>(options.Value.DeadlyBodyParts);
        }

        public async Task<DoResult> Do(SkillContext context, SkillState state)
        {
            float d = await _map.CalculateDistance(context.Doer, context.Parameter.TargetEntity);

            if (d > (float) state.ScriptVar[SkillRange]) return DoResult.OutOfRange();

            if (!await _coolDown.TryDo(context.Doer, CoolDownUpper, (float) state.ScriptVar[CoolDown]))
                return DoResult.CoolingDown();

            // TODO get attacker weapon

            var defenderUnit = _grainFactory.Get<IUnit>(context.Parameter.TargetEntity);
            UnitState attackerState = await _grainFactory.Get<IUnit>(context.Doer).GetData();
            UnitState defenderState = await defenderUnit.GetData();

            UnitProperty dodgeRate = defenderState.DodgeRate;
            UnitProperty hitRate = attackerState.UpperAttackHitRate;
            UnitProperty attackEfficiency = attackerState.UpperAttackEfficiency;

            double hitP = Math.Atan(hitRate / dodgeRate) * 2 / Math.PI;

            if (Rand.NextDouble() > hitP) return DoResult.Missed();

            // TODO use a better hit check
            var bodyPartIndex = (BodyPartIndex) Rand.Next((int) BodyPartIndex.BodyPartCount);
            var wound = new Wound((uint) attackEfficiency, WoundType.Bruise);

            defenderState = await defenderUnit
                               .ApplyWounds(
                                    new Dictionary<BodyPartIndex, Wound> {[bodyPartIndex] = wound}
                                       .ToImmutableDictionary());

            for (var i = 0; i < defenderState.BodyParts.Length; i++)
            {
                if (defenderState.BodyParts[i].Hp != 0 || !_deadlyParts.Contains((BodyPartIndex) i)) continue;

                await defenderUnit.MarkDead(context.Doer);

                return DoResult.Dead(new[] {(BodyPartIndex) i});
            }

            return DoResult.HitBody(new[] {bodyPartIndex});
        }

        public void OnLearned(IEntity learner, SkillState state) { }

        public void OnForgot(IEntity learner, SkillState state) { }
    }
}