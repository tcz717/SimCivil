using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Rpc.Timeout
{
    internal interface IHeartbeatGenerator : IDisposable
    {
        /// <summary>
        /// Is running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Need to sent a heartbeat to server
        /// </summary>
        event EventHandler<EventArgs> HeartbeatNeeded;

        /// <summary>
        /// Start the daemon
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the daemon
        /// </summary>
        void Stop();

        /// <summary>
        /// Notify that a request has been sent
        /// </summary>
        void NotifyPacketSent();
    }
}
