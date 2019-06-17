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
// SimCivil - SimCivil.Orleans.Interfaces - IPhysical.cs
// Update Date: 2019/6/12

using SimCivil.Orleans.Interfaces.Component.State;
using SimCivil.Orleans.Interfaces.Item;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Orleans.Interfaces.Component
{
    public partial interface IPhysical : IItemComponent<PhysicalState>
    {
        /// <summary>
        /// Clears compounds or sub physical parts.
        /// </summary>
        /// <returns></returns>
        Task Clear();

        /// <summary>
        /// Gets the compounds of this item if the item is single part.
        /// </summary>
        /// <returns></returns>
        Task<Result<IDictionary<string, double>>> GetCompounds();

        /// <summary>
        /// Clears and sets the compounds of this item if the item is single part.
        /// </summary>
        /// <param name="compounds">The compounds.</param>
        /// <returns></returns>
        Task<Result> SetCompounds(IDictionary<string, double> compounds);

        /// <summary>
        /// Adds the compounds to this item if the item is single part.
        /// If a compound exists, then adds the weight.
        /// </summary>
        /// <param name="compounds">The compound types and weights.</param>
        /// <returns></returns>
        Task<Result> AddCompounds(IDictionary<string, double> compounds);

        /// <summary>
        /// Update the compounds weight in this item if the item is single part.
        /// </summary>
        /// <param name="compounds">The compound types and weights.</param>
        /// <returns></returns>
        Task<Result> UpdateCompounds(IDictionary<string, double> compounds);

        /// <summary>
        /// Removes the compound from this item if the item is single part.
        /// The compounds may not in the item.
        /// </summary>
        /// <param name="compounds">The compounds to be removed.</param>
        /// <returns></returns>
        Task<Result> RemoveCompound(IEnumerable<string> compounds);

        /// <summary>
        /// Gets the sub physical parts of this item if the item is assembly.
        /// </summary>
        /// <returns></returns>
        Task<Result<IDictionary<string, IPhysicalPart>>> GetPhysicalParts();

        /// <summary>
        /// Clears and sets the sub physical parts of this item if the item is assembly.
        /// </summary>
        /// <param name="subparts">The sub parts.</param>
        /// <returns></returns>
        Task<Result> SetPhysicalParts(IDictionary<string, IPhysicalPart> subparts);

        /// <summary>
        /// Inserts the physical part. This item must be an Assembly.
        /// </summary>
        /// <param name="subparts">The physical part names and parts.</param>
        /// <returns></returns>
        Task<Result> AddPhysicalPart(IDictionary<string, IPhysicalPart> subparts);

        /// <summary>
        /// Update the sub physical parts weight in this item if the item is assembly.
        /// </summary>
        /// <param name="subparts">The physical part names and parts.</param>
        /// <returns></returns>
        Task<Result> UpdatePhysicalPart(IDictionary<string, IPhysicalPart> subparts);

        /// <summary>
        /// Removes the physical part. This item must be an Assembly.
        /// </summary>
        /// <param name="partName">Name of the part.</param>
        /// <returns></returns>
        Task<Result> RemovePhysicalPart(IEnumerable<string> partNames);

        /// <summary>
        /// Determines whether this item is assembly.
        /// </summary>
        /// <returns><c>true</c> if this item is Assembly, else <c>false</c></returns>
        Task<bool> IsAssembly();

        /// <summary>
        /// Determines whether this item is single part.
        /// </summary>
        /// <returns><c>true</c> if this item is SinglePart, else <c>false</c></returns>
        Task<bool> IsSinglePart();

        /// <summary>
        /// Converts to assembly. Convertable iff this item is an Assembly.
        /// </summary>
        /// <returns></returns>
        Task<Result<AssemblyPart>> ToAssemblyPart();

        /// <summary>
        /// Converts to single part. Convertable iff this item is a single part.
        /// </summary>
        /// <returns></returns>
        Task<Result<SinglePart>> ToSinglePart();

        Task<double> GetWeight();

        Task<double> GetVolume();
    }
}
