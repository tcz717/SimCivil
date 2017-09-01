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
        private readonly IEntityRepository entityRepository;

        public int Priority { get; } = 800;

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

        public PlayerController(MapData map, IAuth auth, IEntityRepository entityRepository)
        {
            Map = map;
            this.auth = auth;
            this.entityRepository = entityRepository;
            auth.OnLogined += Auth_OnLogined;
        }

        private void Auth_OnLogined(object sender, Player e)
        {
            entityRepository.LoadPlayerEntity(e);
        }
    }
}
