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
// SimCivil - SimCivil.Orleans.Interfaces - IComponent.cs
// Create Date: 2018/06/14
// Update Date: 2018/12/30

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Orleans;

namespace SimCivil.Orleans.Interfaces
{
    public interface IComponent : IGrainWithGuidKey
    {
        Task<IComponent> CopyTo(IEntity target);
        Task<IReadOnlyDictionary<string, string>> Dump();
        Task<IReadOnlyDictionary<string, string>> Inspect(IEntity observer);
        Task Delete();
    }

    public interface IComponent<T> : IComponent where T : new()
    {
        Task<T> GetData();
        Task SetData(T component);
    }
}