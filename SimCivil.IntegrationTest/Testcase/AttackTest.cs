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
// SimCivil - SimCivil.IntegrationTest - AttackTest.cs
// Create Date: 2019/06/18
// Update Date: 2019/06/19

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Orleans;

using SimCivil.Contract;
using SimCivil.Contract.Model;

namespace SimCivil.IntegrationTest.Testcase
{
    public class AttackTest : EntityTestBase
    {
        public AttackTest(ILogger<AttackTest> logger, IClusterClient cluster) : base(logger, cluster) { }

        public override async Task Test()
        {
            IsRunning = true;
            await UseRole();

            var sync = Client.Import<IViewSynchronizer>();
            var controller = Client.Import<IPlayerController>();

            // This is used to put unit into chunk
            // TODO: Automatically add unit to chunk after login
            await controller.MoveTo((0, 0.01f), DateTime.UtcNow);
            sync.RegisterViewSync(
                vc =>
                {
                    var id = vc.EntityChange?.FirstOrDefault(e => Distance(e.Pos, vc.Position) < 1)?.Id;
                    if ((vc.EntityChange?.Length ?? 0) > 0)
                        Logger.LogInformation(vc.ToString());

                    if (id == null) return;

                    controller.Attack((Guid) id, Guid.Empty, HitMethod.Fist)
                              .ContinueWith(
                                   t => Logger.LogInformation(
                                       "Attack {0}",
                                       t.Result.ToString()));
                });
        }

        private float Distance((float X, float Y) aPos, (float X, float Y) bPos)
        {
            var d = (float) Math.Sqrt(
                (aPos.X - bPos.X) * (aPos.X - bPos.X) + (aPos.Y - bPos.Y) * (aPos.Y - bPos.Y));
            Logger.LogDebug("Distance: {0}", d);

            return d;
        }
    }
}