// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil - SimpleAuth.cs
// Create Date: 2017/09/02
// Update Date: 2018/01/07

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using log4net;

using SimCivil.Contract;
using SimCivil.Rpc;
using SimCivil.Rpc.Session;
using SimCivil.Store;

namespace SimCivil.Auth
{
    /// <summary>
    /// Simple auth just make sure username is unique.
    /// </summary>
    public class SimpleAuth : IAuth, IAuthManager, ISessionRequred
    {
        /// <summary>
        /// Gets the player repository.
        /// </summary>
        /// <value>
        /// The player repository.
        /// </value>
        public IPlayerRepository PlayerRepository { get; }
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor can be injected.
        /// </summary>
        public SimpleAuth(IPlayerRepository playerRepository)
        {
            PlayerRepository = playerRepository;
        }


        /// <summary>
        /// Verify a user token and login.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>login result</returns>
        public virtual bool LogIn(string username, string password)
        {
            // if already online, deny.
            if (OnlinePlayers.Any(p => p.Username == username))
                return false;

            if (!PlayerRepository.IsExsist(username))
            {
                return false;
            }

            Player player = PlayerRepository.GetPlayer(username);

            if (player.Token.ToString() != password)
            {
                return false;
            }

            OnlinePlayers.Add(player);
            Session.Value.Set(player).Exiting += Session_Exiting;
            LoggedIn?.Invoke(this, player);
            Logger.Info($"[{username}] login succeed");

            return true;
        }

        public Task<bool> LogInAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Log out.
        /// </summary>
        public virtual void LogOut()
        {
            Player player = Session.Value.Get<Player>();

            LogOut(player);
        }

        public Task LogOutAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <returns></returns>
        public virtual string GetToken()
        {
            return Session.Value.Get<Player>().Username;
        }

        public Task<bool> Register(string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the online player.
        /// </summary>
        /// <value>
        /// The online player.
        /// </value>
        public IList<Player> OnlinePlayers { get; } = new List<Player>();

        /// <summary>
        /// Happen when user are valid.
        /// </summary>
        public event EventHandler<Player> LoggedIn;

        /// <summary>
        /// Happen when user exits.
        /// </summary>
        public event EventHandler<Player> LoggedOut;
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>
        /// The session.
        /// </value>
        public AsyncLocal<IRpcSession> Session { get; } = new AsyncLocal<IRpcSession>();

        /// <summary>
        /// Logs out.
        /// </summary>
        /// <param name="player">The player.</param>
        protected virtual void LogOut(Player player)
        {
            if (!OnlinePlayers.Remove(player)) return;

            PlayerRepository.UpdatePlayerAsync(player);
            LoggedOut?.Invoke(this, player);
            Logger.Info($"[{player.Username}] logout succeed");
        }

        private void Session_Exiting(object sender, EventArgs e)
        {
            var session = (IRpcSession) sender;
            session.Exiting -= Session_Exiting;
            if (session.IsSet<Player>())
                LogOut(session.Get<Player>());
        }
    }
}