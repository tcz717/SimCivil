using SimCivil.Auth;
using SimCivil.Map;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using SimCivil.Model;

namespace SimCivil.Controller
{
    /// <summary>
    /// Handle all player's action.
    /// </summary>
    public class PlayerController : IController
    {
        private readonly MapData _map;

        /// <inheritdoc />
        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; } = 800;

        /// <summary>
        /// Gets or sets the role entities.
        /// </summary>
        /// <value>
        /// The role entities.
        /// </value>
        public HashSet<Entity> RoleEntities { get; } = new HashSet<Entity>();

        /// <summary>
        /// Updates the specified tick count.
        /// </summary>
        /// <param name="tickCount">The tick count.</param>
        public void Update(int tickCount)
        {
            //TODO
        }

        /// <inheritdoc />
        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            //TODO
        }

        /// <inheritdoc />
        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            //TODO
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public PlayerController(MapData map)
        {
            _map = map;
            _map.Entities.CollectionChanged += Entities_CollectionChanged;
        }

        private void Entities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Entity entity in e.NewItems)
                    {
                        if (RoleEntities.Contains(entity))
                        {
                            throw new InvalidOperationException($"{entity} has existed in {nameof(PlayerController)}");
                        }
                        RoleEntities.Add(entity);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Remove:
                    foreach (Entity entity in e.OldItems)
                    {
                        RoleEntities.Remove(entity);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Replace:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Auth_OnRoleChanged(object sender, RoleChangeArgs e)
        {
            if (e.OldEntity != null) _map.DetachEntity(e.OldEntity);
            _map.AttachEntity(e.NewEntity);
        }
    }
}