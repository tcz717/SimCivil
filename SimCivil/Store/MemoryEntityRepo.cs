using System;
using System.Collections.Generic;
using System.Text;
using SimCivil.Map;
using SimCivil.Model;

namespace SimCivil.Store
{
    /// <summary>
    /// A Entity repository storing entities. Just designed for test.
    /// </summary>
    public class MemoryEntityRepo : IEntityRepository
    {
        public Dictionary<Guid, Entity> Entities { get; set; } = new Dictionary<Guid, Entity>();
        public MemoryEntityRepo()
        {
        }

        public Entity LoadEntity(Guid id)
        {
            return Entities[id];
        }

        public void SaveEntity(Entity entity)
        {
            Entities[entity.Id] = entity;
        }
    }
}
