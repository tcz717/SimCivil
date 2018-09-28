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
// SimCivil - SimCivil.Orleans.Interfaces - IEntity.cs
// Create Date: 2018/06/14
// Update Date: 2018/09/27

using System;
using System.Text;
using System.Threading.Tasks;

using Orleans;

namespace SimCivil.Orleans.Interfaces
{
    public interface IEntity : IGrainWithGuidKey
    {
        Task<bool> Has<T>() where T : IComponent;
        Task Add<T>() where T : IComponent;
        Task Remove<T>() where T : IComponent;

        Task Enable();
        Task Disable();
        Task<bool> IsEnabled();
        Task CopyTo(IEntity targetEntity);
        Task SetName(string name);
        Task<string> GetName();
    }

    public static class EntityExtention
    {
        public static T Get<T>(this IGrainFactory factory, Guid entityId) where T : IComponent
        {
            return factory.GetGrain<T>(entityId);
        }

        public static T Get<T>(this IGrainFactory factory, IEntity entity) where T : IComponent
        {
            return factory.GetGrain<T>(entity.GetPrimaryKey());
        }

        public static T Get<T>(this IGrainFactory factory, IGrainWithGuidKey grain) where T : IComponent
        {
            return factory.GetGrain<T>(grain.GetPrimaryKey());
        }

        public static IEntity GetEntity(this IGrainFactory factory, IGrainWithGuidKey grain)
        {
            return factory.GetGrain<IEntity>(grain.GetPrimaryKey());
        }
    }
}