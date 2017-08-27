using log4net;
using System;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace SimCivil.Net
{
    public class MatrixConnection : IServerConnection, IDisposable
    {
        static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int currentID = 0;

        public MatrixConnection(IServerListener serverListener, Socket socket)
        {
            ServerListener = serverListener;
            Socket = socket;
            Stream = new NetworkStream(socket);
        }

        public IServerListener ServerListener { get; set; }
        public Socket Socket { get; }
        public NetworkStream Stream { get; }

        public event EventHandler<Packet> OnPacketReceived;

        public void Close()
        {
            Stream?.Close();
        }

        public void Dispose()
        {
            Stream?.Dispose();
            Socket?.Dispose();
        }

        public void SendPacket(Packet pkt)
        {
            byte[] data = pkt.ToBytes(currentID);
            Stream.WriteAsync(data, 0, data.Length);
            logger.Debug($"Packet has been sent: \"ID:{pkt.Head.packetID} type:{pkt.Head.type}\"");

            Interlocked.Increment(ref currentID);
        }

        public void TimeOutCheck()
        {
            throw new NotImplementedException();
        }

        ~MatrixConnection()
        {
            Dispose();
        }
    }
}