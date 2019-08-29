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
// SimCivil - SimCivil.Orleans.Grains - SkillGrain.cs
// Create Date: 2019/08/07
// Update Date: 2019/08/07

using System;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Orleans.Interfaces.Skill;

namespace SimCivil.Orleans.Grains.Skill
{
    public class SkillGrain : Grain<SkillState>, ISkill
    {
        public Task<string> GetName() => Task.FromResult(State.Name);

        public Task<DoResult> Do(SkillContext context)
            => GrainFactory.GetGrain<ISkillExecutor>(0, State.ScriptType.ToString()).Do(context, State);

        public Task SetState(SkillState state)
        {
            if (state.Name != this.GetPrimaryKeyString())
                throw new ArgumentException("Skill name inconsistent", nameof(state));

            State = state;

            return WriteStateAsync();
        }

        public Task<SkillState> GetState() => Task.FromResult(State);
    }
}