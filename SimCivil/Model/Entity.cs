using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using SimCivil.Store.Json;
using static SimCivil.Config;

namespace SimCivil.Model
{
    /// <summary>
    /// Represent a game object.
    /// </summary>
    /// <seealso>
    ///     <cref>System.IEquatable{SimCivil.Model.Entity}</cref>
    /// </seealso>
    /// <seealso cref="System.ICloneable" />
    public class Entity : ICloneable, IEquatable<Entity>
    {
        private (int x, int y) _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public Entity(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = "Unknown";

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public (int x, int y) Position
        {
            get => _position;
            set
            {
                if (value.Equals(_position)) return;
                OnPositionChanged(_position, value);
                _position = value;
            }
        }

        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>
        /// The meta.
        /// </value>
        [JsonConverter(typeof(NullableDynamicObjectConverter))]
        public dynamic Meta { get; set; }

        /// <summary>
        /// Occurs when [position changed].
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs<(int X, int Y)>> PositionChanged;

        /// <summary>
        /// Creates new Entity.
        /// </summary>
        /// <returns></returns>
        public static Entity Create()
        {
            return new Entity(Guid.NewGuid())
            {
                Meta = new NullableDynamicObject(),
            };
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Entity Clone()
        {
            return new Entity(Guid.NewGuid())
            {
                Name = Name,
                Position = Cfg.SpawnPoint,
                Meta = Meta.Clone(),
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Called when [position changed].
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnPositionChanged((int X, int Y) oldValue, (int X, int Y) newValue)
        {
            PositionChanged?.Invoke(this, new PropertyChangedEventArgs<(int X, int Y)>(oldValue, newValue));
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
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Entity) obj);
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
    }
}