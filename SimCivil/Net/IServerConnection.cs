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
    }
}