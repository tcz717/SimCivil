using SimCivil.Orleans.Interfaces.Component.State;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Interfaces.Component
{
    public partial interface IContainer : IItemComponent<ContainerState>
    {
        #region StateProperty

        Task<IEnumerable<IEntity>> GetContents();

        Task SetContents(IEnumerable<IEntity> value);

        #endregion

        /// <summary>
        /// Searches the item by the name.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <returns></returns>
        Task<IEnumerable<IEntity>> SearchItemsByName(string itemName);

        /// <summary>
        /// Searches the item by the component.
        /// </summary>
        /// <typeparam name="IComponent">Type of component.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<IEntity>> SearchItemsByComponent<TComponennt>()
            where TComponennt : IComponent;

        /// <summary>
        /// Searches the item by the id.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <returns></returns>
        Task<Result<IEntity>> SearchItem(Guid itemId);

        /// <summary>
        /// Takes the item out of the container.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<Result> TakeItem(IEntity item);

        /// <summary>
        /// Takes the items out of the container.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        Task<Result> TakeItems(IEnumerable<IEntity> items);

        /// <summary>
        /// Takes all the items out of the container.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IEntity>> TakeAll();

        /// <summary>
        /// Puts the item into the container.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task PutItem(IEntity item);

        /// <summary>
        /// Puts the items into the container.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        Task PutItems(IEnumerable<IEntity> items);
    }
}
