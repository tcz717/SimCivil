using SimCivil.Model;
using SimCivil.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Auth
{
    public interface IAuth
    {
        event EventHandler<Player> OnLogined;
        event EventHandler<Player> OnLogouted;
        /// <summary>
        /// Happen when user's role changing.
        /// </summary>
        event EventHandler<RoleChangeArgs> OnRoleChanging;
        /// <summary>
        /// Happen when user's role changed.
        /// </summary>
        event EventHandler<RoleChangeArgs> OnRoleChanged;
        Player Login(string username, object token);
        void Logout(Player player);
        IList<Player> OnlinePlayer { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RoleChangeArgs
    {
        public Entity OldEntity { get; set; }
        public Entity NewEntity { get; set; }
        public Player Player { get; set; }
        public bool Allowed { get; set; }
    }
}
