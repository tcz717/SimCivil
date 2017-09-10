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
        private readonly MapData Map;
        private readonly IAuth auth;

        public int Priority { get; } = 800;

        public void Update(int tickCount)
        {
            //TODO
        }

        public void Start()
        {
            //TODO
        }

        public void Stop()
        {
            //TODO
        }

        public PlayerController(MapData map, IAuth auth)
        {
            Map = map;
            this.auth = auth;
            auth.OnLogined += Auth_OnLogined;
        }

        private void Auth_OnLogined(object sender, Player e)
        {
        }
    }
}
