using Microsoft.Extensions.Logging;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Grains.Component
{
    public class MagicGrain : BaseGrain<MagicalState>, IMagical
    {
        public MagicGrain(ILoggerFactory factory) : base(factory)
        {
            // Default
        }

        public Task AddElements(IDictionary<string, double> elements)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Clear()
        {
            throw new NotImplementedException();
        }
        public Task UpdateElements(IDictionary<string, double> elements)
        {
            throw new NotImplementedException();
        }

        public Task<Result> RemoveElements(IEnumerable<string> elements)
        {
            throw new NotImplementedException();
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
