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
// Create Date: 2019/08/08
// Update Date: 2019/08/12

using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Orleans;

using SimCivil.Contract.Model;
using SimCivil.Orleans.Interfaces;
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
        private          IGrainFactory                     _grainFactory;
        private readonly ICoolDownService                  _coolDown;
        private          ReadOnlyCollection<BodyPartIndex> _deadlyParts;

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

            return DoResult.Success();
        }

        public void OnLearned(IEntity learner, SkillState state) { }

        public void OnForgot(IEntity learner, SkillState state) { }
    }
}