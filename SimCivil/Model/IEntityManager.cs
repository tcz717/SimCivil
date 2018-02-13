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
// SimCivil - SimCivil - IEntityManager.cs
// Create Date: 2018/01/31
// Update Date: 2018/02/01

using System;
using System.Collections.Generic;
using System.Text;

using SimCivil.Components;

namespace SimCivil.Model
{
    /// <summary>
    /// Entity Manager
    /// </summary>
    public interface IEntityManager
    {
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <value>
        /// All.
        /// </value>
        EntityGroup All { get; }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        Entity GetEntity(Guid guid);

        /// <summary>
        /// Attaches the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void AttachEntity(Entity entity);

        /// <summary>
        /// Dettaches the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void DettachEntity(Entity entity);

        /// <summary>
        /// Clones the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Entity CloneEntity(Entity entity);

        /// <summary>
        /// Creates the entity.
        /// </summary>
        /// <returns></returns>
        Entity CreateEntity();


        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void DeleteEntity(Entity entity);

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Entity> GetEntities();

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="componentTypes">The component types.</param>
        /// <returns></returns>
        IEnumerable<Entity> GetEntities(Type[] componentTypes);

        /// <summary>
        /// Groups the specified component types.
        /// </summary>
        /// <param name="componentTypes">The component types.</param>
        /// <returns></returns>
        EntityGroup Group(Type[] componentTypes);
    }

    /// <summary>
    /// 
    /// </summary>
    public static class EntityManagerExtension
    {
        /// <summary>
        /// Groups the specified manager.
        /// </summary>
        /// <typeparam name="TC">The type of the c.</typeparam>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public static EntityGroup Group<TC>(this IEntityManager manager) where TC : IComponent
        {
            return manager.Group(new[] {typeof(TC)});
        }

        /// <summary>
        /// Groups the specified manager.
        /// </summary>
        /// <typeparam name="TC1">The type of the c1.</typeparam>
        /// <typeparam name="TC2">The type of the c2.</typeparam>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public static EntityGroup Group<TC1, TC2>(this IEntityManager manager)
            where TC1 : IComponent where TC2 : IComponent
        {
            return manager.Group(new[] {typeof(TC1), typeof(TC2)});
        }

        /// <summary>
        /// Groups the specified manager.
        /// </summary>
        /// <typeparam name="TC1">The type of the c1.</typeparam>
        /// <typeparam name="TC2">The type of the c2.</typeparam>
        /// <typeparam name="TC3">The type of the c3.</typeparam>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public static EntityGroup Group<TC1, TC2, TC3>(this IEntityManager manager)
            where TC1 : IComponent where TC2 : IComponent where TC3 : IComponent
        {
            return manager.Group(new[] {typeof(TC1), typeof(TC2), typeof(TC3)});
        }

        /// <summary>
        /// Groups the specified manager.
        /// </summary>
        /// <typeparam name="TC1">The type of the c1.</typeparam>
        /// <typeparam name="TC2">The type of the c2.</typeparam>
        /// <typeparam name="TC3">The type of the c3.</typeparam>
        /// <typeparam name="TC4">The type of the c4.</typeparam>
        /// <param name="manager">The manager.</param>
        /// <returns></returns>
        public static EntityGroup Group<TC1, TC2, TC3, TC4>(this IEntityManager manager)
            where TC1 : IComponent where TC2 : IComponent where TC3 : IComponent where TC4 : IComponent
        {
            return manager.Group(new[] {typeof(TC1), typeof(TC2), typeof(TC3), typeof(TC4)});
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="TC">The type of the c.</typeparam>
        /// <returns></returns>
        public static IEnumerable<Entity> GetEntities<TC>(this IEntityManager manager) where TC : IComponent
        {
            return manager.GetEntities(new[] {typeof(TC)});
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="TC1">The type of the c1.</typeparam>
        /// <typeparam name="TC2">The type of the c2.</typeparam>
        /// <returns></returns>
        public static IEnumerable<Entity> GetEntities<TC1, TC2>(this IEntityManager manager)
            where TC1 : IComponent where TC2 : IComponent
        {
            return manager.GetEntities(new[] {typeof(TC1), typeof(TC2)});
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="TC1">The type of the c1.</typeparam>
        /// <typeparam name="TC2">The type of the c2.</typeparam>
        /// <typeparam name="TC3">The type of the c3.</typeparam>
        /// <returns></returns>
        public static IEnumerable<Entity> GetEntities<TC1, TC2, TC3>(this IEntityManager manager)
            where TC1 : IComponent where TC2 : IComponent where TC3 : IComponent
        {
            return manager.GetEntities(new[] {typeof(TC1), typeof(TC2), typeof(TC3)});
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <typeparam name="TC1">The type of the c1.</typeparam>
        /// <typeparam name="TC2">The type of the c2.</typeparam>
        /// <typeparam name="TC3">The type of the c3.</typeparam>
        /// <typeparam name="TC4">The type of the c4.</typeparam>
        /// <returns></returns>
        public static IEnumerable<Entity> GetEntities<TC1, TC2, TC3, TC4>(this IEntityManager manager)
            where TC1 : IComponent where TC2 : IComponent where TC3 : IComponent where TC4 : IComponent
        {
            return manager.GetEntities(new[] {typeof(TC1), typeof(TC2), typeof(TC3), typeof(TC4)});
        }
    }
}