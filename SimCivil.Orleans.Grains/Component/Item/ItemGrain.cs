using Microsoft.Extensions.Logging;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Grains.Component
{
    public class ItemGrain : BaseGrain<ItemState>, IItem
    {
        public ItemGrain(ILoggerFactory factory) : base(factory)
        {
            // Default
        }

        #region StateProperty

        public Task<Orleans.Interfaces.IEntity> GetContainer()
        {
            return Task.FromResult(State.Container);
        }

        public Task SetContainer(Orleans.Interfaces.IEntity value)
        {
            State.Container = value;
            return WriteStateAsync();
        }

        #endregion
    }
}
