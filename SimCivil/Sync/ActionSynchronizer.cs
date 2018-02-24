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
// SimCivil - SimCivil - ActionSynchronizer.cs
// Create Date: 2018/02/11
// Update Date: 2018/02/11

using System;

using SimCivil.Components;
using SimCivil.Model;

namespace SimCivil.Sync
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionSynchronizer : ITicker
    {
        private readonly EntityGroup _movableEntities;
        /// <summary>
        /// Gets the entity manager.
        /// </summary>
        /// <value>
        /// The entity manager.
        /// </value>
        public IEntityManager EntityManager { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionSynchronizer"/> class.
        /// </summary>
        /// <param name="entityManager">The entity manager.</param>
        public ActionSynchronizer(IEntityManager entityManager)
        {
            EntityManager = entityManager;
            _movableEntities = entityManager.Group<MovableComponent, PositionComponent>();
        }
        /// <summary>
        /// Called when tick.
        /// </summary>
        /// <param name="tickCount">Total tick.</param>
        public void Update(int tickCount)
        {
            foreach (Entity entity in _movableEntities.Entities)
            {
                var movable = entity.GetMovable();
                var pos = entity.GetPos();

                // TODO: Add blocking area check.
                pos.Pos = (pos.X + movable.Speed * movable.Direction.X, pos.Y + movable.Speed * movable.Direction.Y);
            }
        }

        /// <summary>
        /// Start Service.
        /// </summary>
        public void Start()
        {
        }

        /// <summary>
        /// Stop Service.
        /// </summary>
        public void Stop()
        {
        }

        /// <summary>
        /// Ticker's priority, larger number has high priorty.
        /// If system is busy, low priorty ticker may be skip.
        /// </summary>
        public int Priority { get; set; } = 900;
    }
}