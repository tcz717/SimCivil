using System;
using System.Net.Sockets;

namespace SimCivil.Net
{
    public class MatrixConnection:IServerConnection
    {
        public MatrixConnection(IServerListener serverListener, Socket socket)
        {
            ServerListener = serverListener;
            Socket = socket;
        }

        public IServerListener ServerListener { get; set; }
        public Socket Socket { get; }

        public event EventHandler<Packet> OnPacketReceived;

        public void SendPacket(Packet pkt)
        {
            throw new NotImplementedException();
        }

        public void TimeOutCheck()
        {
            throw new NotImplementedException();
        }
    }
}