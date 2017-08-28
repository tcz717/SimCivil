using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Auth
{
    public interface IAuth
    {
        event EventHandler<Player> OnLogin;
    }
}
