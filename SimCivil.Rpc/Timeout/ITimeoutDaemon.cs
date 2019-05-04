using DotNetty.Transport.Channels;
using SimCivil.Rpc.Session;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimCivil.Rpc.Timeout
{
    /// <summary>
    /// Timeout Daemon for disconnect clients which do not send packet for a while
    /// </summary>
    public interface ITimeoutDaemon : IDisposable
    {
        /// <summary>
        /// Register Channel in Timeout Daemon. Timeout Daemon will try to emit event to server 
        /// when timeout. The daemon should be running.
        /// </summary>
        /// <param name="channel">The channel to register</param>
        void RegisterChannel(IChannel channel);

        /// <summary>
        /// Unregister channel. Stop taking care of this channel.
        /// The daemon should be running.
        /// </summary>
        /// <param name="channel">The channel to be removed</param>
        void UnregisterChannel(IChannel channel);

        /// <summary>
        /// Start daemon
        /// </summary>
        void Start();

        /// <summary>
        /// Stop daemon
        /// </summary>
        void Stop();

        /// <summary>
        /// whether the daemon is running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Tell daemon that a request is received for a channel
        /// </summary>
        /// <param name="channel">The channel to be notified</param>
        /// <param name="request">The request the channel received</param>
        /// <returns></returns>
        bool NotifyPacketReceived(IChannel channel, RpcRequest request);

        /// <summary>
        /// Emit when a channel is timeout and needs to be closed
        /// </summary>
        event EventHandler<ClientTimeoutEventArgs> ClientTimeout;
    }
}
