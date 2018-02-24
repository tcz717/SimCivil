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
// SimCivil - SimCivil - XYExtension.cs
// Create Date: 2018/02/11
// Update Date: 2018/02/11

using System;

namespace SimCivil
{
    /// <summary>
    /// 
    /// </summary>
    public static class XYExtension
    {
        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns></returns>
        public static float GetLength(this (float X, float Y) tuple)
        {
            return MathF.Sqrt(tuple.X * tuple.X + tuple.Y * tuple.Y);
        }
        /// <summary>
        /// Adds the specified tuple2.
        /// </summary>
        /// <param name="tuple1">The tuple1.</param>
        /// <param name="tuple2">The tuple2.</param>
        /// <returns></returns>
        public static (float X, float Y) Add(this (float X, float Y) tuple1, (float X, float Y) tuple2)
        {
            return (tuple1.X + tuple2.X, tuple1.Y + tuple2.Y);
        }
    }
}