using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Client's login request.
    /// </summary>
    [PacketType(PacketType.Login)]
    public class LoginRequest : Packet
    {
        public LoginRequest(Hashtable data, IServerConnection client) : base(data, client) { }
        /// <summary>
        /// Player's username
        /// </summary>
        public string Username
        {
            get
            {
                return (string)Data[nameof(Username)];
            }
            set
            {
                Data[nameof(Username)] = value;
            }
        }
        /// <summary>
        /// Some token to auth.
        /// </summary>
        public object Token
        {
            get
            {
                return Data[nameof(Token)];
            }
            set
            {
                Data[nameof(Token)] = value;
            }
        }
        /// <summary>
        /// User's client software name (not useranme and optional).
        /// </summary>
        public string ClientName
        {
            get
            {
                return (string)Data[nameof(ClientName)];
            }
            set
            {
                Data[nameof(ClientName)] = value;
            }
        }
        public override bool Verify()
        {
            return base.Verify()
                && Data.ContainsKey(nameof(Username))
                && Data[nameof(Username)] is string
                && Data.ContainsKey(nameof(Token));
        }
    }
}
