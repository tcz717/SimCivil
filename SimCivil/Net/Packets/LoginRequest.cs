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
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequest"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="client">client indicating where to send to or received from</param>
        public LoginRequest(PacketType type, Hashtable data, IServerConnection client) : base(type, data, client)
        {
        }

        /// <summary>
        /// Player's username
        /// </summary>
        public string Username
        {
            get => GetDataProperty<string>();
            set => SetDataProperty(value);
        }

        /// <summary>
        /// Some token to auth.
        /// </summary>
        public object Token
        {
            get => Data[nameof(Token)];
            set => SetDataProperty(value);
        }

        /// <summary>
        /// User's client software name (not useranme and optional).
        /// </summary>
        public string ClientName
        {
            get => GetDataProperty<string>();
            set => SetDataProperty(value);
        }

        /// <summary>
        /// Verify this packet's receiving correctness.
        /// </summary>
        /// <param name="errorDesc"></param>
        /// <returns></returns>
        public override bool Verify(out string errorDesc)
        {
            return base.Verify(out errorDesc)
                   && Data.ContainsKey(nameof(Username))
                   && Data[nameof(Username)] is string
                   && Data.ContainsKey(nameof(Token));
        }
    }
}