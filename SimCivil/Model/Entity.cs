using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static SimCivil.Config;

namespace SimCivil.Model
{
    /// <summary>
    /// Represent a game object.
    /// </summary>
    /// <seealso cref="System.ICloneable" />
    public class Entity : ICloneable
    {
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
        public (int x, int y) Position { get; set; }
        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>
        /// The meta.
        /// </value>
        public dynamic Meta { get; set; }

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
    }
}
