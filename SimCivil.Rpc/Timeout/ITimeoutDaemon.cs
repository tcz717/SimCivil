// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.Rpc - ITimeoutDaemon.cs
// Create Date: 2019/05/08
// Update Date: 2019/05/19

using System;
using System.Text;

using DotNetty.Transport.Channels;

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