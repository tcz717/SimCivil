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
// SimCivil - SimCivil.Orleans.Grains - CooldownGrain.cs
// Create Date: 2019/06/14
// Update Date: 2019/06/16

using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SimCivil.Orleans.Interfaces.Component;

namespace SimCivil.Orleans.Grains.Component
{
    public class CooldownGrain : BaseGrain<CooldownState>, ICooldown
    {
        public CooldownGrain(ILoggerFactory factory) : base(factory) { }

        public async Task<bool> TryDo(string action, TimeSpan cooldown)
        {
            if (State.CooldownItems.TryGetValue(action, out CooldownItem cooldownItem) &&
                cooldownItem.EndTime > DateTime.UtcNow) return false;

            State.CooldownItems[action] = new CooldownItem(action, cooldown);

            await WriteStateAsync();

            return true;
        }

        public Task Reset()
        {
            State.CooldownItems.Clear();

            return WriteStateAsync();
        }

        public Task<DateTime> GetLastDoneTime(string action) => Task.FromResult(
            State.CooldownItems.TryGetValue(action, out CooldownItem cooldown)
                ? cooldown.StartTime
                : DateTime.MinValue);

        public Task<DateTime> GetNextAvailableTime(string action) => Task.FromResult(
            State.CooldownItems.TryGetValue(action, out CooldownItem cooldown)
                ? cooldown.EndTime
                : DateTime.UtcNow);
    }
}