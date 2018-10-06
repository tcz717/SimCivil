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
// Update Date: 2018/10/04

using System;
using System.Text;

using SimCivil.Contract.Model;

namespace SimCivil.Components
{
    /// <summary>
    /// Data related to unit information.
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Components.IComponent</cref>
    /// </seealso>
    public class UnitComponent : BaseComponent
    {
        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public Gender Gender { get; set; }
        /// <summary>
        /// Gets or sets the race.
        /// </summary>
        /// <value>
        /// The race.
        /// </value>
        public Race Race { get; set; }

        /// <summary>
        /// Gets or sets the move speed.
        /// </summary>
        /// <value>
        /// The move speed.
        /// </value>
        public float MoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the sight range.
        /// </summary>
        /// <value>
        /// The sight range.
        /// </value>
        public float SightRange { get; set; }
    }
}