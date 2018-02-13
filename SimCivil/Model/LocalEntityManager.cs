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
// SimCivil - SimCivil - LocalEntityManager.cs
// Create Date: 2018/01/31
// Update Date: 2018/02/01

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using JetBrains.Annotations;

namespace SimCivil.Model
{
    /// <inheritdoc />
    public class LocalEntityManager : IEntityManager
    {
        private readonly ConcurrentDictionary<Guid, Entity> _entities = new ConcurrentDictionary<Guid, Entity>();

        private readonly Dictionary<int, EntityGroup> _groups = new Dictionary<int, EntityGroup>();
        private readonly ConcurrentStack<Entity> _inactiveEntities = new ConcurrentStack<Entity>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalEntityManager"/> class.
        /// </summary>
        public LocalEntityManager()
        {
            All = new EntityGroup();
            _groups.Add(All.GetHashCode(), All);
        }


        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public Entity GetEntity(Guid guid)
        {
            return _entities.GetValueOrDefault(guid);
        }

        /// <summary>
        /// Attaches the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void AttachEntity(Entity entity)
        {
            _entities[entity.Id] = entity;
            entity.CollectionChanged += Entity_ComponentChanged;
            foreach (var entityGroup in _groups.Values)
            {
                entityGroup.TryAdd(entity);
            }
        }

        private void Entity_ComponentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Entity entity = (Entity) sender;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var entityGroup in _groups.Values)
                    {
                        entityGroup.TryAdd(entity);
                    }

                    break;
                case NotifyCollectionChangedAction.Move:

                    break;
                case NotifyCollectionChangedAction.Remove:

                    break;
                case NotifyCollectionChangedAction.Replace:

                    break;
                case NotifyCollectionChangedAction.Reset:

                    break;
                default:

                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Dettaches the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void DettachEntity(Entity entity)
        {
            entity.CollectionChanged -= Entity_ComponentChanged;
            _entities.Remove(entity.Id, out entity);
            foreach (var entityGroup in _groups.Values)
            {
                entityGroup.Remove(entity);
            }
        }

        /// <summary>
        /// Clones the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public Entity CloneEntity(Entity entity)
        {
            if (_inactiveEntities.TryPop(out Entity clone))
            {
                entity.Clone(clone);
            }
            else
            {
                clone = entity.Clone();
            }

            AttachEntity(clone);

            return clone;
        }

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <returns></returns>
        public Entity CreateEntity()
        {
            if (_inactiveEntities.TryPop(out Entity entity))
            {
                entity.Reset();
            }
            else
            {
                entity = Entity.Create();
            }

            AttachEntity(entity);

            return entity;
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void DeleteEntity(Entity entity)
        {
            DettachEntity(entity);
            _inactiveEntities.Push(entity);
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Entity> GetEntities()
        {
            return All.Entities;
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="componentTypes">The component types.</param>
        /// <returns></returns>
        public IEnumerable<Entity> GetEntities(Type[] componentTypes)
        {
            return Group(componentTypes).Entities;
        }

        /// <summary>
        /// Groups the specified component types.
        /// </summary>
        /// <param name="componentTypes">The component types.</param>
        /// <returns></returns>
        public EntityGroup Group(Type[] componentTypes)
        {
            if (_groups.TryGetValue(EntityGroup.GetHashCode(componentTypes), out EntityGroup entityGroup))
            {
                return entityGroup;
            }

            entityGroup = new EntityGroup(componentTypes);
            foreach (Entity entity in _entities.Values)
            {
                entityGroup.TryAdd(entity);
            }

            _groups.Add(EntityGroup.GetHashCode(componentTypes), entityGroup);

            return entityGroup;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <value>
        /// All.
        /// </value>
        [NotNull]
        public EntityGroup All { get; }
    }
}