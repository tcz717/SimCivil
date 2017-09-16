using System;
using System.Collections.Generic;
using System.Text;
using SimCivil.Map;
using SimCivil.Model;

namespace SimCivil.Store
{
    /// <inheritdoc />
    /// <summary>
    /// A Entity repository storing entities. Just designed for test.
    /// </summary>
    public class MemoryEntityRepo : IEntityRepository
    {
        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        public Dictionary<Guid, Entity> Entities { get; set; } = new Dictionary<Guid, Entity>();

        /// <summary>
        /// Loads the entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Entity LoadEntity(Guid id)
        {
            return Entities[id];
        }

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void SaveEntity(Entity entity)
        {
            Entities[entity.Id] = entity;
        }
    }
}
