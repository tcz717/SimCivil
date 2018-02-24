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
// SimCivil - SimCivil - MovableComponent.cs
// Create Date: 2018/02/09
// Update Date: 2018/02/10

using System;
using System.Text;

using SimCivil.Model;

namespace SimCivil.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso>
    ///     <cref>SimCivil.Components.BaseComponent</cref>
    /// </seealso>
    public class MovableComponent : BaseComponent
    {
        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public (float X, float Y) Direction { get; set; }
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>
        /// The speed.
        /// </value>
        public float Speed { get; set; }
    }

    public static partial class EntityExtenstion
    {
        private static readonly string MovableName = typeof(MovableComponent).FullName;

        /// <summary>
        /// Gets the MovableComponent.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static MovableComponent GetMovable(this Entity entity)
        {
            return (MovableComponent) entity.Components[MovableName];
        }
    }
}