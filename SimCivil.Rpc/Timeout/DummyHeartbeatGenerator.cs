using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimCivil.Rpc.Timeout
{
    /// <summary>
    /// Heartbeat generation indication
    /// </summary>
    internal class DummyHeartbeatGenerator : IHeartbeatGenerator
    {
        private CancellationTokenSource _cancel;
        private Task _runTask;
        private bool _sentPacket;
        private readonly RpcClient _client;

        /// <summary>
        /// Is running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Need to sent a heartbeat to server
        /// </summary>
        public event EventHandler<EventArgs> HeartbeatNeeded;

        public DummyHeartbeatGenerator(RpcClient client)
        {
            // Dummy
        }

        /// <summary>
        /// Start the daemon
        /// </summary>
        public void Start()
        {
            // Dummy
        }

        /// <summary>
        /// Stop the daemon
        /// </summary>
        public void Stop()
        {
            // Dummy
        }

        /// <summary>
        /// Notify that a request has been sent
        /// </summary>
        public void NotifyPacketSent()
        {
            // Dummy
        }

        public void Dispose()
        {
            // Dummy
        }
    }
}
