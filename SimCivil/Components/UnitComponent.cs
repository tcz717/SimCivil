// Copyright (c) 2017 TPDT
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
// SimCivil - SimCivil - UnitComponent.cs
// Create Date: 2018/01/07
// Update Date: 2018/01/07

using System;

using SimCivil.Contract.Model;

namespace SimCivil.Components
{
    /// <summary>
    /// Data related to unit infomation.
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Components.IComponent</cref>
    /// </seealso>
    public class UnitComponent : IComponent
    {
        public Guid EntityId { get; set; }

        /// <summary>
        /// Clones with specified new identifier.
        /// </summary>
        /// <param name="newId">The new identifier.</param>
        /// <returns></returns>
        public IComponent Clone(Guid newId)
        {
            UnitComponent component = (UnitComponent) MemberwiseClone();
            component.EntityId = newId;

            return component;
        }

        public string Name { get; set; }
        public Gender Gender { get; set; }
        public Race Race { get; set; }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            return Clone(EntityId);
        }
    }
}