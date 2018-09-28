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
// SimCivil - SimCivil.Orleans.Interfaces - IPlayerController.cs
// Create Date: 2018/07/11
// Update Date: 2018/09/26

using System;
using System.Text;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Contract;

namespace SimCivil.Orleans.Interfaces.Component
{
    public interface IUnitController : IComponent
    {
        /// <summary>
        /// Moves the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        Task Move((float X, float Y) direction, float speed);

        Task Stop();

        Task Drop(IEntity target);

        Task Attack(IEntity target);

        Task Use(IEntity target);

        Task<EntityInspection> Inspect(IEntity target);
        Task<(float X, float Y)> GetSpeed();
    }
}