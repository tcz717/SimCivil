using SimCivil.Net.Packets;
using System;

namespace SimCivil.Net
{
    public interface IServerConnection
    {
        IServerListener ServerListener { get; set; }

        event EventHandler OnDisconnected;
        event EventHandler<Packet> OnPacketReceived;

        void TimeOutCheck();
        void SendPacket(Packet pkt);
        void Close();

        bool Connected { get; }

        /// <summary>
        /// Used to send a packet and request it's reply for next step.
        /// </summary>
        /// <typeparam name="T">ResponsePacket</typeparam>
        /// <param name="packet">Packet to send</param>
        /// <param name="callback">callback wehen received specified response.</param>
        void SendAndWait<T>(Packet packet, Action<T> callback) where T : ResponsePacket;
    }
}