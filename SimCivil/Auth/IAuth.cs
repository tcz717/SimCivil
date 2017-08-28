using SimCivil.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Auth
{
    public interface IAuth
    {
        event EventHandler<Player> OnLogined;
        bool Login(string username, object token, IServerConnection connection = null);
        IList<Player> OnlinePlayer { get; }
    }
}
