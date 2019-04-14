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
// SimCivil - SimCivil.Gate - OrleansPlayerController.cs
// Create Date: 2018/09/27
// Update Date: 2018/10/17

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Orleans;

using SimCivil.Contract;
using SimCivil.Orleans.Interfaces;
using SimCivil.Orleans.Interfaces.Component;
using SimCivil.Rpc;
using SimCivil.Rpc.Session;

namespace SimCivil.Gate
{
    [LoginFilter]
    public class OrleansPlayerController : IPlayerController, ISessionRequired
    {
        public IGrainFactory Factory { get; }

        public OrleansPlayerController(IGrainFactory factory)
        {
            Factory = factory;
        }

        /// <summary>
        /// Moves the specified direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public async Task Move((float X, float Y) direction, float speed)
        {
            var controller = Factory.GetGrain<IUnitController>(Session.Value.Get<IEntity>().GetPrimaryKey());

            await controller.Move(direction, speed);
        }

        public async Task Stop()
        {
            var controller = Factory.GetGrain<IUnitController>(Session.Value.Get<IEntity>().GetPrimaryKey());
            await controller.Stop();
        }

        public AsyncLocal<IRpcSession> Session { get; set; } = new AsyncLocal<IRpcSession>();

        public async Task MoveTo((float X, float Y) position, DateTime timestamp)
        {
            var controller = Factory.GetGrain<IUnitController>(Session.Value.Get<IEntity>().GetPrimaryKey());

            await controller.MoveTo(new Position(position), timestamp);
        }

        public async Task<InspectionResult> Inspect(Guid entityId)
        {
            var controller = Factory.GetGrain<IUnitController>(Session.Value.Get<IEntity>().GetPrimaryKey());

            return await controller.InspectEntity(Factory.GetEntity(entityId));
        }
    }
}