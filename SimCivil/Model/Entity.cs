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
// SimCivil - SimCivil - Entity.cs
// Create Date: 2017/08/25
// Update Date: 2018/02/02

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using Newtonsoft.Json;

using SimCivil.Components;

namespace SimCivil.Model
{
    /// <summary>
    /// Represent a game object.
    /// </summary>
    /// <seealso>
    ///     <cref>System.IEquatable{SimCivil.Model.Entity}</cref>
    /// </seealso>
    /// <seealso cref="System.ICloneable" />
    [JsonObject(ItemTypeNameHandling = TypeNameHandling.Auto)]
    public class Entity : ICloneable, IEquatable<Entity>, INotifyCollectionChanged
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; protected set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the components.
        /// </summary>
        /// <value>
        /// The components.
        /// </value>
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Auto)]
        public Dictionary<string, IComponent> Components { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified type name.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="name">Name of the type.</param>
        /// <returns></returns>
        public IComponent this[string name]
        {
            get => Components[name];
            set
            {
                Components.TryGetValue(name, out IComponent oldValue);

                if (ReferenceEquals(oldValue, value)) return;

                Components[name] = value;
                if (oldValue == null)
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
                else if (value == null)
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldValue));
                else
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldValue));
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Entity"/> is dirty (has some change not sync).
        /// </summary>
        /// <value>
        ///   <c>true</c> if dirty; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool Dirty { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="IComponent"/> with the specified type.
        /// </summary>
        /// <value>
        /// The <see cref="IComponent"/>.
        /// </value>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IComponent this[Type type]
        {
            get => Components[type.FullName];
            set => this[type.FullName] = value;
        }

        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>
        /// The meta.
        /// </value>
        [JsonExtensionData]
        public Dictionary<string, object> Meta { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Entity(Guid id)
        {
            Id = id;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Entity other)
        {
            if (other is null) return false;

            return ReferenceEquals(this, other) || Id.Equals(other.Id);
        }

        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        /// <returns></returns>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Clones the specified clone to.
        /// </summary>
        /// <param name="cloneTo">The clone to.</param>
        public void Clone(Entity cloneTo)
        {
            cloneTo.Reset();
            foreach (var (key, component) in Components)
            {
                cloneTo.Components.Add(key, component);
            }

            foreach (var (key, m) in Meta)
            {
                cloneTo.Meta.Add(key, m);
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            Id = Guid.NewGuid();
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, Components.Values));
            Components.Clear();
            Meta.Clear();
            Dirty = true;
        }

        /// <summary>
        /// Gets component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : class
        {
            return this[typeof(T)] as T;
        }

        /// <summary>
        /// Determines whether [has].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        ///   <c>true</c> if [has]; otherwise, <c>false</c>.
        /// </returns>
        public bool Has<T>()
        {
            return Has(typeof(T));
        }

        /// <summary>
        /// Sets the component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component">The component.</param>
        public void Set<T>(T component) where T : IComponent
        {
            this[typeof(T)] = component;
        }

        /// <summary>
        /// Adds new component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Entity Add<T>() where T : IComponent, new()
        {
            T component = new T {EntityId = Id};
            Set(component);

            return this;
        }

        /// <summary>
        /// Adds new component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="init">The initialize.</param>
        /// <returns></returns>
        public Entity Add<T>(Action<T> init) where T : IComponent, new()
        {
            T component = new T {EntityId = Id};
            Set(component);
            init(component);

            return this;
        }

        /// <summary>
        /// Adds the specified component.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <returns></returns>
        public Entity Add(IComponent component)
        {
            component.EntityId = Id;
            Set(component);

            return this;
        }

        /// <summary>
        /// Creates new Entity.
        /// </summary>
        /// <returns></returns>
        public static Entity Create()
        {
            return new Entity(Guid.NewGuid())
            {
                Meta = new Dictionary<string, object>(),
                Components = new Dictionary<string, IComponent>(),
                Dirty = true
            };
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Entity Clone()
        {
            return Clone(false);
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Entity Clone(bool full)
        {
            Guid id = Guid.NewGuid();

            return new Entity(id)
            {
                Meta = full ? new Dictionary<string, object>(Meta) : new Dictionary<string, object>(),
                Components = new Dictionary<string, IComponent>(
                    Components.Select(
                        n => new KeyValuePair<string, IComponent>(n.Key, n.Value.Clone(id)))),
                Dirty = true
            };
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Entity) obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Id.GetHashCode();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Components)}: {Components.Count}, {nameof(Dirty)}: {Dirty}, {nameof(Meta)}: {Meta.Count}";
        }

        /// <summary>
        /// Determines whether [has] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if [has] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public bool Has(Type type)
        {
            return Components.ContainsKey(type.FullName);
        }

        /// <summary>
        /// Raises the <see cref="E:CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Tries to get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGet<T>(out T component) where T : class
        {
            if (!Components.ContainsKey(typeof(T).FullName))
            {
                component = null;
                return false;
            }

            component = this[typeof(T)] as T;
            return true;
        }
    }
}