using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Concept.ItemModel
{
    interface IContainer : IItem
    {
        Task<IEnumerable<IItem>> GetContents();

        Task<IEnumerable<IItem>> SearchItem(string itemName);

        Task<IItem> SearchItem(Guid itemId);

        /// <summary>
        /// Takes the item out of the container.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task TakeItem(IItem item);

        Task RemoveItem(IItem item);

        Task RemoveItems(IEnumerable<IItem> item);

        Task RemoveAll();
    }
}
