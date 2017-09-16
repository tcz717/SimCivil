using SimCivil.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static SimCivil.Config;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// Packet show it's version.
    /// </summary>
    [PacketType(PacketType.Handshake)]
    public class Handshake : Packet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Handshake"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="client">client indicating where to send to or received from</param>
        public Handshake(PacketType type, Hashtable data, IServerConnection client) : base(type, data, client)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Handshake"/> class.
        /// </summary>
        /// <param name="auth">The authentication.</param>
        public Handshake(IAuth auth)
        {
            Protocol = SimCivilProtocolVersion.ToString();
            GameName = Cfg.Name;
            ServerVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Auth = auth.GetType().Name;
        }

        /// <summary>
        /// Protocol version.
        /// </summary>
        public string Protocol
        {
            get => (string) Data[nameof(Protocol)];
            set => Data[nameof(Protocol)] = value;
        }

        /// <summary>
        /// Game name.
        /// </summary>
        public string GameName
        {
            get => (string) Data[nameof(GameName)];
            set => Data[nameof(GameName)] = value;
        }

        /// <summary>
        /// Server's version.
        /// </summary>
        public string ServerVersion
        {
            get => (string) Data[nameof(ServerVersion)];
            set => Data[nameof(ServerVersion)] = value;
        }

        /// <summary>
        /// Server's auto type.
        /// </summary>
        public string Auth
        {
            get => (string) Data[nameof(Auth)];
            set => Data[nameof(Auth)] = value;
        }

        /// <summary>
        /// The method executed after clients received and pushed in the PacketReadQueue
        /// </summary>
        public override void Handle()
        {
            ReplyError(2, "Handshake should be sent from server.");
            base.Handle();
        }
    }
}