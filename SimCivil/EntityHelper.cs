using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SimCivil.Auth;
using SimCivil.Model;
using SimCivil.Store;

namespace SimCivil {
    /// <summary>
    /// Helper class for Entity
    /// </summary>
    public static class EntityHelper
    {
        /// <summary>
        /// Loads the player roles.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public static IEnumerable<Entity> LoadPlayerRoles(this IEntityRepository repository, Player player)
        {
            return player.Roles.Select(repository.LoadEntity);
        }
    }
}