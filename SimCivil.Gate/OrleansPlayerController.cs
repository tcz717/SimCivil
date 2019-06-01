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
// Create Date: 2019/05/13
// Update Date: 2019/05/31

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

        public AsyncLocal<IRpcSession> Session { get; set; } = new AsyncLocal<IRpcSession>();

        public async Task<(float X, float Y)> MoveTo((float X, float Y) position, DateTime timestamp)
        {
            var controller = Factory.GetGrain<IUnitController>(Session.Value.Get<IEntity>().GetPrimaryKey());

            return await controller.MoveTo(new PositionState(position), timestamp);
        }

        public async Task<InspectionResult> Inspect(Guid entityId)
        {
            var controller = Factory.GetGrain<IUnitController>(Session.Value.Get<IEntity>().GetPrimaryKey());

            return await controller.InspectEntity(Factory.GetEntity(entityId));
        }
    }
}