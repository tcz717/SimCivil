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
// SimCivil - SimCivil - EventConsumerComponent.cs
// Create Date: 2018/01/31
// Update Date: 2018/01/31

using System;
using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

using Newtonsoft.Json;

using SimCivil.Contract;

namespace SimCivil.Components
{
    /// <inheritdoc />
    public class ObserverComponent : BaseComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObserverComponent"/> class.
        /// </summary>
        /// <param name="notifyRange">The notify range.</param>
        public ObserverComponent(uint notifyRange)
        {
            NotityRange = notifyRange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObserverComponent"/> class.
        /// </summary>
        public ObserverComponent() : this(0) { }

        /// <summary>
        /// Gets or sets the notity range.
        /// </summary>
        /// <value>
        /// The notity range.
        /// </value>
        public uint NotityRange { get; set; }

        /// <summary>
        /// Gets or sets the tile change.
        /// </summary>
        /// <value>
        /// The tile change.
        /// </value>
        [JsonIgnore]
        public List<TileDto> TileChange { get; } = new List<TileDto>();
        /// <summary>
        /// Gets or sets the entity change.
        /// </summary>
        /// <value>
        /// The entity change.
        /// </value>
        [JsonIgnore]
        public List<EntityDto> EntityChange { get; } = new List<EntityDto>();
        /// <summary>
        /// Gets or sets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        [JsonIgnore]
        public List<ViewEvent> Events { get; } = new List<ViewEvent>();
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>
        /// The callback.
        /// </value>
        [CanBeNull]
        [JsonIgnore]
        public Action<ViewChange> Callback { get; set; }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Events.Clear();
            EntityChange.Clear();
            TileChange.Clear();
        }
    }
}