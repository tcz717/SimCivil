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
        Player Login(string username, object token);
        void Logout(Player player);
        IList<Player> OnlinePlayer { get; }
    }
}
