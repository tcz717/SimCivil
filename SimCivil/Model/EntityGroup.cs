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
// SimCivil - SimCivil - EntityGroup.cs
// Create Date: 2018/01/31
// Update Date: 2018/02/02

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SimCivil.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityGroup
    {
        /// <summary>
        /// Gets the component types.
        /// </summary>
        /// <value>
        /// The component types.
        /// </value>
        public Type[] ComponentTypes { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public HashSet<Entity> Entities { get; } = new HashSet<Entity>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityGroup"/> class.
        /// </summary>
        public EntityGroup() : this(new Type[] { }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityGroup"/> class.
        /// </summary>
        /// <param name="componentTypes">The component types.</param>
        public EntityGroup(Type[] componentTypes)
        {
            ComponentTypes = componentTypes;
            Id = GetHashCode(ComponentTypes);
        }

        /// <summary>
        /// Occurs when [entity added].
        /// </summary>
        public event EventHandler<Entity> EntityAdded;
        /// <summary>
        /// Occurs when [entity removed].
        /// </summary>
        public event EventHandler<Entity> EntityRemoved;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="componentTypes">The component types.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public static int GetHashCode(Type[] componentTypes)
        {
            return componentTypes.Aggregate(0, (current, componentType) => current ^ componentType.GetHashCode());
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Id;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{nameof(ComponentTypes)}: {ComponentTypes}, {nameof(Id)}: {Id}";
        }

        /// <summary>
        /// Tries the add.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void TryAdd(Entity entity)
        {
            if (Entities.Contains(entity)) return;
            if (!ComponentTypes.All(entity.Has)) return;

            entity.CollectionChanged += Entity_ComponentChanged;
            Entities.Add(entity);
            OnEntityAdded(entity);
        }

        private void Entity_ComponentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Entity entity = (Entity)sender;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    break;
                case NotifyCollectionChangedAction.Move:

                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (!ComponentTypes.All(entity.Has))
                    {
                        Remove(entity);
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:

                    break;
                case NotifyCollectionChangedAction.Reset:
                    Remove(entity);
                    break;
                default:

                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Tries the remove.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Remove(Entity entity)
        {
            entity.CollectionChanged -= Entity_ComponentChanged;
            Entities.Remove(entity);
            OnEntityRemoved(entity);
        }

        /// <summary>
        /// Called when [entity added].
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void OnEntityAdded(Entity e)
        {
            EntityAdded?.Invoke(this, e);
        }

        /// <summary>
        /// Called when [entity removed].
        /// </summary>
        /// <param name="e">The e.</param>
        protected virtual void OnEntityRemoved(Entity e)
        {
            EntityRemoved?.Invoke(this, e);
        }
    }
}