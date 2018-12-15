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
// SimCivil - SimCivil.Orleans.Interfaces - SyncOption.cs
// Create Date: 2018/12/13
// Update Date: 2018/12/13

using System;

namespace SimCivil.Orleans.Interfaces.Option
{
    public class SyncOptions
    {
        /// <summary>
        /// Gets or sets the size of the chunk.
        /// </summary>
        /// <value>
        /// The size of the chunk.
        /// </value>
        public int ChunkSize { get; set; } = 32;
        /// <summary>
        /// Gets or sets the lag learning rate.
        /// </summary>
        /// <value>
        /// The lag learning rate.
        /// </value>
        public double LagLearningRate { get; set; } = 0.9;
        /// <summary>
        /// Gets or sets the maximum lag.
        /// </summary>
        /// <value>
        /// The maximum lag.
        /// </value>
        public double MaxLag { get; set; } = 1000;
        /// <summary>
        /// Gets or sets the maximum update delta.
        /// </summary>
        /// <value>
        /// The maximum update delta.
        /// </value>
        public TimeSpan MaxUpdateDelta { get; set; } = TimeSpan.FromMilliseconds(200);
        /// <summary>
        /// Gets or sets a value indicating whether [no speed fix].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [no speed fix]; otherwise, <c>false</c>.
        /// </value>
        public bool NoSpeedFix { get; set; } = false;

        /// <summary>
        /// Gets or sets the update period.
        /// </summary>
        /// <value>
        /// The update period.
        /// </value>
        public int UpdatePeriod { get; set; } = 50;
    }
}