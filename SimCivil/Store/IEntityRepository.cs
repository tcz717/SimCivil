using SimCivil.Auth;
using SimCivil.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Store
{
    /// <summary>
    /// Repository Store Entities
    /// </summary>
    public interface IEntityRepository
    {
        /// <summary>
        /// Loads the entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Entity LoadEntity(Guid id);
        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void SaveEntity(Entity entity);
    }
}
