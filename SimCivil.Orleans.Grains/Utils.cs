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
// SimCivil - SimCivil.Orleans.Grains - Utils.cs
// Create Date: 2018/09/26
// Update Date: 2018/10/05

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using Orleans;

namespace SimCivil.Orleans.Grains
{
    public static class Utils
    {
        private static readonly Dictionary<Type, Func<Guid, IGrainWithGuidKey>> CacheFuncs =
            new Dictionary<Type, Func<Guid, IGrainWithGuidKey>>();

        public static (float X, float Y) Normalize((float X, float Y) v)
        {
            float len = (float) Math.Sqrt(v.X * v.X + v.Y * v.Y);

            if (len < 0.0001f)
                return (0, 0);

            return (v.X / len, v.Y / len);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static T GetGrain<T>(this IGrainFactory factory, Type type, Guid guid)
        {
            if (CacheFuncs.TryGetValue(type, out var func))
            {
                return (T) func(guid);
            }

            MethodInfo method = typeof(IGrainFactory).GetMethod(nameof(GetGrain), new[] {typeof(Guid), typeof(string)})
                .MakeGenericMethod(type);
            ParameterExpression arg0 = Expression.Parameter(typeof(Guid));
            var newFunc = Expression.Lambda<Func<Guid, IGrainWithGuidKey>>(
                    Expression.Call(
                        Expression.Constant(factory),
                        method,
                        arg0,
                        Expression.Constant(null, typeof(string))),
                    arg0)
                .Compile();
            CacheFuncs.Add(type, newFunc);

            return (T) newFunc(guid);
        }
    }
}