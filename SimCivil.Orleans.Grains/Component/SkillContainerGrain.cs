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
// SimCivil - SimCivil.Orleans.Grains - SkillContainerGrain.cs
// Create Date: 2019/08/05
// Update Date: 2019/08/06

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using SimCivil.Orleans.Interfaces.Skill;

namespace SimCivil.Orleans.Grains.Component
{
    public class SkillContainerGrain : Grain<SkillContainerState>, ISkillContainer
    {
        public Task<IComponent> CopyTo(IEntity target) => throw new NotImplementedException();

        public async Task<IReadOnlyDictionary<string, string>> Dump()
        {
            var item = await Task.WhenAll(State.Skills.Keys.Select(skill => skill.GetName()).ToArray());

            return new Dictionary<string, string> {["Skills"] = string.Join(", ", item)};
        }

        public Task<IReadOnlyDictionary<string, object>> Inspect(IEntity observer)
            => Task.FromResult<IReadOnlyDictionary<string, object>>(
                new Dictionary<string, object> {["Skills"] = State.Skills.ToArray()});

        public Task Delete()
        {
            State = null;

            return ClearStateAsync();
        }

        public Task<bool> Has(ISkill skill) => Task.FromResult(State.Skills.ContainsKey(skill));

        public Task Learn(ISkill skill)
        {
            State.Skills.Add(skill, 0);

            return WriteStateAsync();
        }

        public async Task<bool> Forget(ISkill skill)
        {
            bool result = State.Skills.Remove(skill);
            await WriteStateAsync();

            return result;
        }

        public Task<int> GetProficiency(ISkill skill) => Task.FromResult(State.Skills[skill]);
    }
}