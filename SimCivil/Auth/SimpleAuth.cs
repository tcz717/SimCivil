using log4net;
using SimCivil.Net;
using SimCivil.Net.Packets;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimCivil.Auth
{
    /// <summary>
    /// Simple auth just make sure username is unique.
    /// </summary>
    public class SimpleAuth : IAuth
    {
        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Happen when user are vaild.
        /// </summary>
        public event EventHandler<Player> OnLogined;

        /// <summary>
        /// Constrcutor can be injected.
        /// </summary>
        /// <param name="server"></param>
        public SimpleAuth(IServerListener server)
        {
            server.OnConnected += Server_OnConnected;
        }

        private void Server_OnConnected(object sender, IServerConnection e)
        {
            e.SendAndWait<OkResponse>(new Handshake(this), resp =>
             {
                 logger.Info($"Handshake ok with ${resp.Client}");
             });
        }
    }
}
