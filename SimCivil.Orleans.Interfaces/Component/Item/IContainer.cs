// Copyright (c) 2019 TPDT
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
// SimCivil - SimCivil.Orleans.Interfaces - IContainer.cs
// Update Date: 2019/6/12

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
