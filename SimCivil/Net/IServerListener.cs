using System;
using System.Collections.Concurrent;
using System.Net;

namespace SimCivil.Net
{
    public interface IServerListener
    {
        void SendPacket(Packet pkt);
        void Start();
        void Stop();
        void AttachClient(IServerConnection client);
        void DetachClient(IServerConnection client);
        void RegisterPacket(PacketType type, PacketCallBack callBack);
        void UnregisterPacket(PacketType type, PacketCallBack callBack);

        /// <summary>
        /// The event triggered when a new ServerClient created
        /// </summary>
        event EventHandler<IServerConnection> OnConnected;
        /// <summary>
        /// The event triggered when a connection closed
        /// </summary>
        event EventHandler<IServerConnection> OnDisconnected;
    }

    public delegate void PacketCallBack(Packet pkt, ref bool isVaild);
}