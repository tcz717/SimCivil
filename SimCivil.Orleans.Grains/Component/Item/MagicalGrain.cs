using Microsoft.Extensions.Logging;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Grains.Component
{
    public class MagicGrain : BaseGrain<MagicalState>, IMagical
    {
        public MagicGrain(ILoggerFactory factory) : base(factory)
        {
            // Default
        }

        public async Task AddElements(IDictionary<string, double> elements)
        {
            foreach (var ele in elements)
            {
                if (ele.Value > 0)
                {
                    State.ElementQuantities[ele.Key] = (State.ElementQuantities.TryGetValue(ele.Key, out double val) ? val : 0) + ele.Value;
                }
            }

            await WriteStateAsync();
            return;
        }

        public async Task Clear()
        {
            State = new MagicalState();
            await WriteStateAsync();
        }

        public async Task UpdateElements(IDictionary<string, double> elements)
        {
            foreach (var ele in elements)
            {
                if (ele.Value > 0)
                {
                    State.ElementQuantities[ele.Key] = ele.Value;
                }
                else
                {
                    State.ElementQuantities.Remove(ele.Key);
                }
            }

            await WriteStateAsync();
            return;
        }

        public async Task<Result> RemoveElements(IEnumerable<string> elements)
        {
            var conflicts = new List<string>();
            foreach (var ele in elements)
            {
                if (!State.ElementQuantities.Remove(ele))
                {
                    conflicts.Add(ele);
                }
            }

            await WriteStateAsync();
            if (conflicts.Count > 0)
            {
                return new Result(ErrorCode.PartiallyComplete, $"Elements not found: [{string.Join(",", conflicts)}]");
            }

            return new Result();
        }

        #region StateProperty

        public Task<IDictionary<string, double>> GetElementQuantities()
        {
            return Task.FromResult(State.ElementQuantities);
        }

        public Task SetElementQuantities(IDictionary<string, double> value)
        {
            State.ElementQuantities = value;
            return WriteStateAsync();
        }

        #endregion
    }
}
