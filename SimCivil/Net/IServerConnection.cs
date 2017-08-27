using System;

namespace SimCivil.Net
{
    public interface IServerConnection
    {
        IServerListener ServerListener { get; set; }

        event EventHandler<Packet> OnPacketReceived;
        void TimeOutCheck();
        void SendPacket(Packet pkt);
    }
}