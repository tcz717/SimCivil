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
        Entity LoadEntity(Guid id);
        void SaveEntity(Guid id, Entity entity);
    }
}
