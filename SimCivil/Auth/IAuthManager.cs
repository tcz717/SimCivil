using System;
using System.Collections.Generic;

namespace SimCivil.Auth
{
    public interface IAuthManager
    {
        IList<Player> OnlinePlayer { get; }

        event EventHandler<Player> LoggedIn;
        event EventHandler<Player> LoggedOut;
    }
}