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
// SimCivil - SimCivil - ChunkViewSynchronizer.cs
// Create Date: 2018/01/31
// Update Date: 2018/02/05

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using AutoMapper;

using SimCivil.Auth;
using SimCivil.Components;
using SimCivil.Contract;
using SimCivil.Model;
using SimCivil.Rpc;
using SimCivil.Rpc.Session;

using static System.Math;

namespace SimCivil.Sync
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IViewSynchronizer" />
    [LoginFilter]
    public class ChunkViewSynchronizer : IViewSynchronizer, ITicker, ISessionRequred
    {
        private static readonly IMapper Mapper =
            new Mapper(
                new MapperConfiguration(
                    cfg =>
                    {
                        cfg.CreateMap<Entity, EntityDto>()
                            .ForMember(dto => dto.Pos, n => n.MapFrom(e => e.GetPos().Pos));
                        cfg.CreateMap<ObserverComponent, ViewChange>();
                    }));

        private static readonly (int X, int Y)[] Offsets =
            {(0, 0), (1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, -1), (1, -1), (-1, 1)};

        private readonly ConcurrentQueue<Entity> _changedEntities = new ConcurrentQueue<Entity>();

        private readonly Dictionary<(int, int), HashSet<Entity>>
            _chunks = new Dictionary<(int, int), HashSet<Entity>>();

        /// <summary>
        /// Gets the entity manager.
        /// </summary>
        /// <value>
        /// The entity manager.
        /// </value>
        public IEntityManager EntityManager { get; }

        /// <summary>
        /// Gets or sets the event consumer.
        /// </summary>
        /// <value>
        /// The event consumer.
        /// </value>
        public EntityGroup ObserverGroup { get; set; }

        /// <summary>
        /// Gets or sets the size of the chunk.
        /// </summary>
        /// <value>
        /// The size of the chunk.
        /// </value>
        public int ChunkSize { get; set; } = 30;

        /// <summary>
        /// Gets or sets the event producer.
        /// </summary>
        /// <value>
        /// The event producer.
        /// </value>
        public EntityGroup MovableEntities { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkViewSynchronizer"/> class.
        /// </summary>
        /// <param name="entityManager">The entity manager.</param>
        public ChunkViewSynchronizer(IEntityManager entityManager)
        {
            EntityManager = entityManager;
            ObserverGroup = entityManager.Group<PositionComponent, ObserverComponent>();
            MovableEntities = entityManager.Group<PositionComponent>();
            foreach (Entity entity in MovableEntities.Entities)
            {
                AddProduer(entity);
            }

            MovableEntities.EntityAdded += EventConsumer_EntityAdded;
            MovableEntities.EntityRemoved += EventConsumer_EntityRemoved;
        }

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        public ThreadLocal<IRpcSession> Session { get; set; }

        /// <summary>
        /// Called when tick.
        /// </summary>
        /// <param name="tickCount">Total tick.</param>
        public void Update(int tickCount)
        {
            var changedEntities = new HashSet<Entity>();
            while (_changedEntities.TryDequeue(out Entity entity))
            {
                changedEntities.Add(entity);
            }

            foreach (Entity entity in MovableEntities.Entities)
            {
                entity.GetPos().Sync();
            }

            foreach (Entity changedEntity in changedEntities)
            {
                var position = changedEntity.GetPos();
                (int X, int Y) prevChunk = GetChunkIdx(position.PreviousPos);
                (int X, int Y) currentChunk = GetChunkIdx(position.Pos);

                MoveChunk(changedEntity, prevChunk, currentChunk);

                var effectChunks = Offsets.Select(o => (prevChunk.X + o.X, prevChunk.Y + o.Y))
                    .Union(Offsets.Select(o => (currentChunk.X + o.X, currentChunk.Y + o.Y)));

                foreach ((int, int) effectChunk in effectChunks)
                {
                    if (!_chunks.TryGetValue(effectChunk, out var entities)) continue;

                    foreach (Entity effectedEntity in entities)
                    {
                        if (Equals(effectedEntity, changedEntity)) continue;

                        if (effectedEntity.Has<ObserverComponent>())
                            UpdateChange(changedEntity, effectedEntity);
                        if (changedEntity.Has<ObserverComponent>())
                            UpdateChange(effectedEntity, changedEntity);
                    }
                }
            }

            foreach (Entity entity in ObserverGroup.Entities)
            {
                var consumer = entity.Get<ObserverComponent>();

                if (consumer.Callback is null) continue;

                var viewChange = Mapper.Map<ViewChange>(consumer);
                viewChange.TickCount = tickCount;
                consumer.Callback(viewChange);
                consumer.Reset();
            }
        }

        private static void UpdateChange(Entity sourceEntity, Entity targetEntity)
        {
            var targetPos = targetEntity.GetPos();
            var sourcePos = sourceEntity.GetPos();
            var observerComponent = targetEntity.Get<ObserverComponent>();
            float prevDistance = Max(
                Abs(targetPos.PreviousPos.X - sourcePos.PreviousPos.X),
                Abs(targetPos.PreviousPos.Y - sourcePos.PreviousPos.Y));
            float currentDistance = Max(Abs(targetPos.X - sourcePos.X), Abs(targetPos.Y - sourcePos.Y));
            uint range = observerComponent.NotityRange;

            if (currentDistance < range)
            {
                observerComponent.EntityChange.Add(
                    Mapper.Map<EntityDto>(sourceEntity));
            }
            else if (prevDistance < range)
            {
                observerComponent.Events.Add(ViewEvent.EntityLeave(sourceEntity.Id));
            }
        }

        /// <summary>
        /// Start Service.
        /// </summary>
        public void Start() { }

        /// <summary>
        /// Stop Service.
        /// </summary>
        public void Stop() { }

        /// <summary>
        /// Ticker's priority, larger number has high priorty.
        /// If system is busy, low priorty ticker may be skip.
        /// </summary>
        public int Priority { get; set; } = 999;

        /// <summary>
        /// Synchronize the partial view.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void RegisterViewSync(Action<ViewChange> callback)
        {
            var role = Session.Value.Get<Entity>();
            if (!role.Has<ObserverComponent>())
            {
                throw new NotSupportedException(nameof(ObserverGroup));
            }

            role.Get<ObserverComponent>().Callback = callback;
        }

        private void MoveChunk(Entity entity, (int X, int Y) prevChunk, (int X, int Y) currentChunk)
        {
            if (currentChunk.Equals(prevChunk)) return;

            if (_chunks.TryGetValue(prevChunk, out var chunk))
            {
                chunk.Remove(entity);
            }

            if (_chunks.TryGetValue(currentChunk, out chunk))
            {
                chunk.Add(entity);
            }
            else
            {
                _chunks.Add(currentChunk, new HashSet<Entity> {entity});
            }
        }

        private void EventConsumer_EntityRemoved(object sender, Entity e)
        {
            e.GetPos().PropertyChanged -= Component_PropertyChanged;
        }

        private void EventConsumer_EntityAdded(object sender, Entity e) => AddProduer(e);

        private void AddProduer(Entity entity)
        {
            var position = entity.GetPos();
            (int X, int Y) chunkIdx = GetChunkIdx(position.Pos);
            if (_chunks.TryGetValue(chunkIdx, out var entities))
            {
                entities.Add(entity);
            }
            else
            {
                entities = new HashSet<Entity> {entity};
                _chunks.Add(chunkIdx, entities);
            }

            OnEntityChanged(entity);
            entity.GetPos().PropertyChanged += Component_PropertyChanged;
        }

        private void Component_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Pos")
            {
                OnEntityChanged(EntityManager.GetEntity(((IComponent) sender).EntityId));
            }
        }

        private (int X, int Y) GetChunkIdx((float, float) pos)
        {
            var(x, y) = pos;

            return ((int) Floor(x) / ChunkSize, (int) Floor(y) / ChunkSize);
        }

        private void OnEntityChanged(Entity entity)
        {
            _changedEntities.Enqueue(entity);
        }
    }
}