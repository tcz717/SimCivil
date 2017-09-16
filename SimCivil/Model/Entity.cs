using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using static SimCivil.Config;

namespace SimCivil.Model
{
    /// <summary>
    /// Represent a game object.
    /// </summary>
    /// <seealso cref="System.ICloneable" />
    public class Entity : ICloneable
    {
        private (int x, int y) _position;

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

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
            return new Entity()
            {
                Id = Guid.NewGuid(),
                Meta = new NullableDynamicObject(),
            };
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Entity Clone()
        {
            return new Entity()
            {
                Id = Guid.NewGuid(),
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
    }
}