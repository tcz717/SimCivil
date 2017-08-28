using SimCivil.Auth;
using SimCivil.Map;
using SimCivil.Net;
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
        private readonly MapData Map;
        private readonly IAuth auth;

        public void Update(int tickCount)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public PlayerController(MapData map, IAuth auth)
        {
            Map = map;
            this.auth = auth;
            auth.OnLogined += Auth_OnLogined;
        }

        private void Auth_OnLogined(object sender, Player e)
        {
            throw new NotImplementedException();
        }
    }
}
