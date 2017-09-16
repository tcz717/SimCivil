using Newtonsoft.Json;
using SimCivil.Model;
using SimCivil.Net;
using System;
using System.Collections.Generic;

namespace SimCivil.Auth
{
    /// <summary>
    /// Represent a human player.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="token">The token.</param>
        public Player(string username, object token)
        {
            Username = username;
            Token = token;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player() { }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public object Token { get; set; }
        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        public string PlayerName { get; set; }
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
        public List<Guid> Roles { get; set; }
        /// <summary>
        /// Gets or sets the current role.
        /// </summary>
        /// <value>
        /// The current role.
        /// </value>
        [JsonIgnore]
        public Entity CurrentRole { get; set; }
    }
}