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
// SimCivil - SimCivil.Orleans.Interfaces - IMagical.cs
// Update Date: 2019/6/12

using SimCivil.Orleans.Interfaces.Component.State;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Interfaces.Component
{
    public partial interface IMagical : IItemComponent<MagicalState>
    {
        #region StateProperty

        Task<IDictionary<string, double>> GetElementQuantities();

        Task SetElementQuantities(IDictionary<string, double> value);

        #endregion

        /// <summary>
        /// Clears elements.
        /// </summary>
        /// <returns></returns>
        Task<Result> Clear();

        /// <summary>
        /// Adds the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task AddElements(IDictionary<string, double> elements);

        /// <summary>
        /// Updates the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task UpdateElements(IDictionary<string, double> elements);

        /// <summary>
        /// Removes the magic elements.
        /// </summary>
        /// <param name="elements">The elements.</param>
        /// <returns></returns>
        Task<Result> RemoveElements(IEnumerable<string> elements);
    }
}
