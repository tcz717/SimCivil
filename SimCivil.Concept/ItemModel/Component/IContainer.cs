using SimCivil.Orleans.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IContainer : IComponent
    {
        /// <summary>
        /// Gets all the names of items.
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<IEntity>>> GetAll();

        /// <summary>
        /// Searches the item by the name.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <returns></returns>
        Task<Result<IEnumerable<IEntity>>> SearchItem(string itemName);

        /// <summary>
        /// Searches the item by the id.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <returns></returns>
        Task<IEntity> SearchItem(Guid itemId);

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
        Task<Result<IEnumerable<IEntity>>> TakeAll();

        /// <summary>
        /// Puts the item into the container.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<Result> PutItem(IEntity item);

        /// <summary>
        /// Puts the items into the container.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        Task<Result> PutItems(IEnumerable<IEntity> items);
    }
}
