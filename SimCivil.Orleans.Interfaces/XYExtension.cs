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
// SimCivil - SimCivil.Orleans.Interfaces - XYExtension.cs
// Create Date: 2018/12/15
// Update Date: 2018/12/17

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Orleans;

namespace SimCivil.Orleans.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public static class XYExtension
    {
        public static readonly (int X, int Y)[] Offsets =
            {(0, 0), (1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, -1), (1, -1), (-1, 1)};

        /// <summary>
        /// Gets the length.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <returns></returns>
        public static float GetLength(this (float X, float Y) tuple)
        {
            return (float) Math.Sqrt(tuple.X * tuple.X + tuple.Y * tuple.Y);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ModPositive(this int a, int b)
        {
            return (a % b + b) % b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int DivDown(this int a, int b)
        {
            return (a - (a % b + b) % b) / b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int X, int Y) ModPositive(this (int X, int Y) a, int b)
        {
            return ((a.X % b + b) % b, (a.Y % b + b) % b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int X, int Y) DivDown(this (int X, int Y) a, int b)
        {
            return ((a.X - (a.X % b + b) % b) / b, (a.Y - (a.Y % b + b) % b) / b);
        }

        public static
            TGrainInterface GetGrain<TGrainInterface>(
                this IGrainFactory factory,
                (int X, int Y) primaryKey,
                string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey
        {
            return factory.GetGrain<TGrainInterface>(
                ((long) primaryKey.X) & uint.MaxValue | ((long) primaryKey.Y << 32),
                grainClassNamePrefix);
        }

        public static (int X, int Y) GetPrimaryKeyXY(this IGrain grain)
        {
            long key = grain.GetPrimaryKeyLong();

            return ((int X, int Y)) (key & uint.MaxValue, key >> 32);
        }

        public static IEnumerable<(int X, int Y)> ForAround(this (int X, int Y) center)
        {
            return Offsets.Select(o => (center.X + o.X, center.Y + o.Y));
        }
    }
}