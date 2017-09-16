using Autofac;
using SimCivil.Auth;
using SimCivil.Model;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCivil
{
    /// <summary>
    /// Helper class for Autofac
    /// </summary>
    public static class ContainerHelper
    {
        /// <summary>
        /// Call all services' one method at once.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="container">Service container.</param>
        /// <param name="action">Method to be called</param>
        public static void CallMany<T>(this IContainer container, Action<T> action)
        {
            var servces = container.Resolve<IEnumerable<T>>();
            foreach (var service in servces)
            {
                action(service);
            }
        }
    }

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