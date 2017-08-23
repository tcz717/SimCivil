using SimCivil.Map;
using SimCivil.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Controller
{
    public class PlayerController : IController
    {
        private MapData Map;
        private IServerListener Server;

        public void Update(int tickCount)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            Server.OnNewConnected += Server_OnNewConnected;
        }

        private void Server_OnNewConnected(object sender, ServerClient e)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            Server.OnNewConnected -= Server_OnNewConnected;
        }

        public PlayerController(MapData map, IServerListener server)
        {
            Map = map;
            Server = server;
        }
    }
}
