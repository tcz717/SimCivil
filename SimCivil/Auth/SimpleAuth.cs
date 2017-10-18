using log4net;
using SimCivil.Model;
using SimCivil.Net;
using SimCivil.Net.Packets;
using SimCivil.Store;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SimCivil.Auth
{
    /// <summary>
    /// Simple auth just make sure username is unique.
    /// </summary>
    public class SimpleAuth : IAuth
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Happen when user are valid.
        /// </summary>
        public event EventHandler<Player> LoggedIn;

        /// <summary>
        /// Happen when user exits.
        /// </summary>
        public event EventHandler<Player> LoggedOut;

        /// <summary>
        /// Happen when user's role changing.
        /// </summary>
        public event EventHandler<RoleChangeArgs> RoleChanging;

        /// <summary>
        /// Happen when user's role changed.
        /// </summary>
        public event EventHandler<RoleChangeArgs> RoleChanged;


        private readonly HashSet<IServerConnection> _readyToLogin;
        private readonly IEntityRepository _entityRepository;

        /// <summary>
        /// Gets the online player.
        /// </summary>
        /// <value>
        /// The online player.
        /// </value>
        public IList<Player> OnlinePlayer { get; } = new List<Player>();

        /// <summary>
        /// Constructor can be injected.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="entityRepository"></param>
        public SimpleAuth(IServerListener server, IEntityRepository entityRepository)
        {
            _readyToLogin = new HashSet<IServerConnection>();
            server.OnConnected += Server_OnConnected;
            server.OnDisconnected += Server_OnDisconnected;
            server.RegisterPacket(PacketType.Login, LoginHandle);
            server.RegisterPacket(PacketType.QueryRoleList, QueryRoleListHandle);
            server.RegisterPacket(PacketType.SwitchRole, SwitchRoleHandle);
            server.RegisterPacket(PacketType.GenerateRole, GenerateRoleHandle);
            _entityRepository = entityRepository;
        }

        private void GenerateRoleHandle(Packet pkt, ref bool isValid)
        {
            if (isValid)
            {
                GenerateRole request = pkt as GenerateRole;
                Entity role = Entity.Create();
                Debug.Assert(request != null, nameof(request) + " != null");
                role.Name = request.RoleName;
                role.Meta.Sex = request.Sex;
            }
        }

        private void SwitchRoleHandle(Packet pkt, ref bool isValid)
        {
            SwitchRole request = pkt as SwitchRole;
            Debug.Assert(request != null, nameof(request) + " != null");
            if (isValid)
            {
                if (SwitchRole(pkt, _entityRepository.LoadEntity(request.RoleGuid)))
                {
                    pkt.ReplyOk();
                }
                else
                {
                    pkt.ReplyDeny();
                }
            }
        }

        /// <summary>
        /// Switches the role.
        /// </summary>
        /// <param name="pkt">The packet.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public bool SwitchRole(Packet pkt, Entity entity)
        {
            RoleChangeArgs args = new RoleChangeArgs()
            {
                NewEntity = entity,
                OldEntity = pkt.Client.ContextPlayer.CurrentRole,
                Player = pkt.Client.ContextPlayer,
                Allowed = true,
            };
            RoleChanging?.Invoke(this, args);

            if (args.Allowed)
            {
                if (args.OldEntity != null)
                    _entityRepository.SaveEntity(args.OldEntity);
                args.Player.CurrentRole = args.NewEntity;

                RoleChanged?.Invoke(this, args);

                logger.Info($"Switched role of {args.Player} to {args.NewEntity}");
            }
            else
            {
                logger.Info($"{args.Player} switches role fail.");
            }
            return args.Allowed;
        }

        private void Server_OnDisconnected(object sender, IServerConnection e)
        {
            if (_readyToLogin.Contains(e))
                _readyToLogin.Remove(e);
            if (e.ContextPlayer == null)
                return;
            Logout(e.ContextPlayer);
            e.ContextPlayer = null;
        }

        /// <summary>
        /// Logout the specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <inheritdoc />
        public void Logout(Player player)
        {
            if (!OnlinePlayer.Remove(player)) return;
            LoggedOut?.Invoke(this, player);
            logger.Info($"[{player.Username}] logout succeed");
        }

        private void QueryRoleListHandle(Packet pkt, ref bool isValid)
        {
            if (isValid)
                pkt.Reply(new QueryRoleListResponse(_entityRepository.LoadPlayerRoles(pkt.Client.ContextPlayer)));
        }

        private void LoginHandle(Packet p, ref bool isValid)
        {
            LoginRequest pkt = p as LoginRequest;
            if (isValid)
            {
                if (!_readyToLogin.Contains(p.Client))
                {
                    isValid = false;
                    p.ReplyError(desc: "Handshake responses first.");
                    return;
                }
                Debug.Assert(pkt != null, nameof(pkt) + " != null");
                Player player = Login(pkt.Username, pkt.Token);
                if (player != null)
                {
                    p.Client.ContextPlayer = player;
                    p.ReplyOk();
                }
                else
                {
                    isValid = false;
                    p.ReplyError(2, "Player has logged in");
                }
            }
            _readyToLogin.Remove(p.Client);
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
            LoggedIn?.Invoke(this, player);
            logger.Info($"[{username}] login succeed");
            return player;
        }

        private void Server_OnConnected(object sender, IServerConnection e)
        {
            e.SendAndWait<OkResponse>(new Handshake(this), resp =>
            {
                logger.Info($"Handshake OK with {resp.Client}");
                _readyToLogin.Add(e);
            });
        }
    }
}