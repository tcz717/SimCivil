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
// Update Date: 2018/01/08

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Newtonsoft.Json;

using SimCivil.Components;
using SimCivil.Store.Json;

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
    public class Entity : ICloneable, IEquatable<Entity>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; }

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
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public IComponent this[string typeName]
        {
            get => Components[typeName];
            set => Components[typeName] = value;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Entity"/> is dirty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if dirty; otherwise, <c>false</c>.
        /// </value>
        [JsonIgnore]
        public bool Dirty { get; private set; } = false;

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified type.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IComponent this[Type type]
        {
            get => Components[type.FullName];
            set => Components[type.FullName] = value;
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
        /// Gets component.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : class
        {
            return this[typeof(T)] as T;
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
        /// Creates new Entity.
        /// </summary>
        /// <returns></returns>
        public static Entity Create()
        {
            return new Entity(Guid.NewGuid())
            {
                Meta = new Dictionary<string, object>(),
                Components = new Dictionary<string, IComponent>()
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
                        n => new KeyValuePair<string, IComponent>(n.Key, n.Value.Clone(id))))
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
            return Id.GetHashCode();
        }

        /// <summary>
        /// Gets the data property.
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">message - propertyName</exception>
        protected T GetMetaProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("message", nameof(propertyName));
            }

            return (T) Meta[propertyName];
        }

        /// <summary>
        /// Sets the data property.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <exception cref="System.ArgumentException">message - propertyName</exception>
        protected void SetMetaProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("message", nameof(propertyName));
            }

            Meta[propertyName] = value;
        }
    }
}