using SimCivil.Auth;
using SimCivil.Map;
using SimCivil.Net;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Controller
{
    /// <summary>
    /// Handle all player's action.
    /// </summary>
    public class PlayerController : IController
    {
        private readonly MapData _map;
        private readonly IAuth _auth;

        /// <inheritdoc />
        /// <summary>
        /// Gets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; } = 800;

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
        /// <param name="auth">The authentication.</param>
        public PlayerController(MapData map, IAuth auth)
        {
            _map = map;
            _auth = auth;
            auth.OnRoleChanged += Auth_OnRoleChanged;
        }

        private void Auth_OnRoleChanged(object sender, RoleChangeArgs e)
        {
            throw new NotImplementedException();
        }
    }
}