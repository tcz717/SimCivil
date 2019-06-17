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
    public class ContainerGrain : BaseGrain<ContainerState>, IContainer
    {
        public ContainerGrain(ILoggerFactory factory) : base(factory)
        {
            // Default
        }

        public Task PutItem(IEntity item)
        {
            throw new NotImplementedException();
        }

        public Task PutItems(IEnumerable<IEntity> items)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEntity>> SearchItem(Guid itemId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> SearchItemsByComponent<TComponennt>() where TComponennt : IComponent
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> SearchItemsByName(string itemName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEntity>> TakeAll()
        {
            throw new NotImplementedException();
        }

        public Task<Result> TakeItem(IEntity item)
        {
            throw new NotImplementedException();
        }

        public Task<Result> TakeItems(IEnumerable<IEntity> items)
        {
            throw new NotImplementedException();
        }

        #region StateProperty

        public Task<IEnumerable<IEntity>> GetContents()
        {
            return Task.FromResult(State.Contents);
        }

        public Task SetContents(IEnumerable<IEntity> value)
        {
            State.Contents = value;
            return WriteStateAsync();
        }

        #endregion
    }
}
