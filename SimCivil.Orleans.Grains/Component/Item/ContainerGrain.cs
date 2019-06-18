using Microsoft.Extensions.Logging;
using Orleans;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Orleans.Interfaces.Component.State;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Grains.Component
{
    public class ContainerGrain : BaseGrain<ContainerState>, IContainer
    {
        public const string ItemNotFound = "No item found for the condition.";

        public ContainerGrain(ILoggerFactory factory, IGrainFactory grainFactory) : base(factory)
        {
        }

        public async Task PutItem(IEntity item)
        {
            State.Contents.Add(item);
            await WriteStateAsync();
            return;
        }

        public async Task PutItems(IEnumerable<IEntity> items)
        {
            foreach (var item in items)
            {
                State.Contents.Add(item);
            }

            await WriteStateAsync();
            return;
        }

        public Task<Result<IEntity>> SearchItem(Guid itemId)
        {
            // TODO: Add cache dict for faster searching
            foreach (var item in State.Contents)
            {
                if (item.GetPrimaryKey() == itemId)
                {
                    return Task.FromResult(new Result<IEntity>(item));
                }
            }

            return Task.FromResult(new Result<IEntity>(ErrorCode.ItemNotFound, ItemNotFound));
        }

        public async Task<IList<IEntity>> SearchItemsByComponent<TComponennt>() where TComponennt : IComponent
        {
            var list = new List<IEntity>();

            // TODO: Add cache dict for faster searching
            foreach (var item in State.Contents)
            {
                if (await item.Has<TComponennt>())
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public async Task<IList<IEntity>> SearchItemsByName(string itemName)
        {
            var list = new List<IEntity>();

            // TODO: Add cache dict for faster searching
            foreach (var item in State.Contents)
            {
                if (await item.GetName() == itemName)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public Task<IList<IEntity>> TakeAll()
        {
            var list = State.Contents;
            State = new ContainerState();

            return Task.FromResult(list);
        }

        public Task<Result> TakeItem(IEntity item)
        {
            if (State.Contents.Remove(item))
            {
                return Task.FromResult(new Result());
            }

            return Task.FromResult(new Result(ErrorCode.ItemNotFound, ItemNotFound));
        }

        public Task<Result> TakeItems(IEnumerable<IEntity> items)
        {
            var conflicts = new List<string>();

            foreach (var item in items)
            {
                if (!State.Contents.Remove(item))
                {
                    conflicts.Add(item.GetPrimaryKey().ToString());
                }
            }

            if (conflicts.Count > 0)
            {
                return Task.FromResult(new Result(ErrorCode.PartiallyComplete, $"Items not found: [{string.Join(",", conflicts)}]"));
            }

            return Task.FromResult(new Result());
        }

        #region StateProperty

        public Task<IList<IEntity>> GetContents()
        {
            return Task.FromResult(State.Contents);
        }

        public Task SetContents(IList<IEntity> value)
        {
            State.Contents = value;
            return WriteStateAsync();
        }

        #endregion
    }
}
