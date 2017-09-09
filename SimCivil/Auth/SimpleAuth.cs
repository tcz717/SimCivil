using log4net;
using SimCivil.Net;
using SimCivil.Net.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public event EventHandler<Player> OnLogouted;

        HashSet<IServerConnection> readyToLogin;
        public IList<Player> OnlinePlayer { get; private set; } = new List<Player>();
        /// <summary>
        /// Constrcutor can be injected.
        /// </summary>
        /// <param name="server"></param>
        public SimpleAuth(IServerListener server)
        {
            readyToLogin = new HashSet<IServerConnection>();
            server.OnConnected += Server_OnConnected;
            server.OnDisconnected += Server_OnDisconnected;
            server.RegisterPacket(PacketType.Login, LoginHandle);
            server.RegisterPacket(PacketType.QueryRoleList, QueryRoleListHandle);
        }

        private void Server_OnDisconnected(object sender, IServerConnection e)
        {
            if (e.ContextPlayer == null)
                return;
            Logout(e.ContextPlayer);
            e.ContextPlayer = null;
        }

        public void Logout(Player player)
        {
            if(OnlinePlayer.Remove(player))
            {
                OnLogouted?.Invoke(this, player);
                logger.Info($"[{player.Username}] logout succeed");
            }
        }

        private void QueryRoleListHandle(Packet pkt, ref bool isVaild)
        {
            if (isVaild)
                pkt.Reply(new QueryRoleListResponse());
        }

        private void LoginHandle(Packet p, ref bool isVaild)
        {
            LoginRequest pkt = p as LoginRequest;
            if (isVaild)
            {
                if (!readyToLogin.Contains(p.Client))
                {
                    isVaild = false;
                    p.ReplyError(desc: "Handshake responses first.");
                    return;
                }
                Player player = Login(pkt.Username, pkt.Token);
                if (player != null)
                {
                    p.Client.ContextPlayer = player;
                    p.ReplyOk();
                }
                else
                {
                    isVaild = false;
                    p.ReplyError(2, "Player has logined");
                }
            }
            readyToLogin.Remove(p.Client);
        }

        /// <summary>
        /// Verify a user token and login.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="token"></param>
        /// <returns>login result</returns>
        public Player Login(string username, object token)
        {
            // if already online, deny.
            if (OnlinePlayer.Any(p => p.Username == username))
                return null;
            Player player = new Player(username, token);
            OnlinePlayer.Add(player);
            OnLogined?.Invoke(this, player);
            logger.Info($"[{username}] login succeed");
            return player;
        }

        private void Server_OnConnected(object sender, IServerConnection e)
        {
            e.SendAndWait<OkResponse>(new Handshake(this), resp =>
             {
                 logger.Info($"Handshake ok with ${resp.Client}");
                 readyToLogin.Add(e);
             });
        }
    }
}
