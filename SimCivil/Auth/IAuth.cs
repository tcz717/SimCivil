using SimCivil.Model;
using SimCivil.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Auth
{
    /// <summary>
    /// Player Auth System
    /// </summary>
    public interface IAuth
    {
        /// <summary>
        /// Occurs when [on logined].
        /// </summary>
        event EventHandler<Player> OnLogined;

        /// <summary>
        /// Occurs when [on logouted].
        /// </summary>
        event EventHandler<Player> OnLogouted;

        /// <summary>
        /// Happen when user's role changing.
        /// </summary>
        event EventHandler<RoleChangeArgs> OnRoleChanging;

        /// <summary>
        /// Happen when user's role changed.
        /// </summary>
        event EventHandler<RoleChangeArgs> OnRoleChanged;

        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Player Login(string username, object token);

        /// <summary>
        /// Logouts the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        void Logout(Player player);

        /// <summary>
        /// Gets the online player.
        /// </summary>
        /// <value>
        /// The online player.
        /// </value>
        IList<Player> OnlinePlayer { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RoleChangeArgs
    {
        /// <summary>
        /// Gets or sets the old entity.
        /// </summary>
        /// <value>
        /// The old entity.
        /// </value>
        public Entity OldEntity { get; set; }

        /// <summary>
        /// Gets or sets the new entity.
        /// </summary>
        /// <value>
        /// The new entity.
        /// </value>
        public Entity NewEntity { get; set; }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public Player Player { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RoleChangeArgs"/> is allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if allowed; otherwise, <c>false</c>.
        /// </value>
        public bool Allowed { get; set; }
    }
}