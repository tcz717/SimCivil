using System.Collections.Generic;
using System.Net;

namespace SimCivil.Net
{
    public interface IServerListener
    {
        Dictionary<EndPoint, ServerClient> Clients { get; }
        int Port { get; set; }

        void SendPacket(Packet pkt);
        void Start();
    }
}