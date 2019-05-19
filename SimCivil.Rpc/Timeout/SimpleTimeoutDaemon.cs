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
// SimCivil - SimCivil.Rpc - SimpleTimeoutDaemon.cs
// Create Date: 2019/05/08
// Update Date: 2019/05/19

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using DotNetty.Transport.Channels;

using Microsoft.Extensions.Logging;

using static System.Diagnostics.Debug;

namespace SimCivil.Rpc.Timeout
{
    /// <summary>
    /// One clock hand algorithm timeout daemon
    /// </summary>
    public class SimpleTimeoutDaemon : ITimeoutDaemon
    {
        private readonly int                              _waitTime;
        public event EventHandler<ClientTimeoutEventArgs> ClientTimeout;

        protected List<KeyValuePair<IChannel, EntryValue>> ClientsToBeRemoved =
            new List<KeyValuePair<IChannel, EntryValue>>();

        protected readonly RpcServer               Server;
        protected readonly ILogger                 Logger;
        protected          Task                    Daemon;
        protected          CancellationTokenSource CancelSrc;
        public             bool                    IsRunning { get; private set; }

        private readonly ConcurrentDictionary<IChannel, EntryValue> _receiveCounts =
            new ConcurrentDictionary<IChannel, EntryValue>();

        public SimpleTimeoutDaemon(RpcServer server, ILogger logger, int waitTime)
        {
            Server    = server ?? throw new ArgumentNullException(nameof(server));
            Logger    = logger ?? throw new ArgumentNullException(nameof(logger));
            _waitTime = waitTime;
        }

        public virtual bool NotifyPacketReceived(IChannel channel, RpcRequest request)
        {
            if (!_receiveCounts.TryGetValue(channel, out EntryValue cnt))
            {
                Logger.LogInformation($"Notify a timeout channel: {channel.RemoteAddress}");

                return false;
            }

            lock (cnt)
            {
                if (!_receiveCounts.TryGetValue(channel, out EntryValue _))
                {
                    Logger.LogInformation("Channel found but after lock it is timeout");

                    return false;
                }

                cnt.Count++;
                Logger.LogDebug($"Session cnt increased to {cnt.Count}, channel: {channel.RemoteAddress}");
            }

            return true;
        }

        public virtual void RegisterChannel(IChannel channel)
        {
            Assert(IsRunning, "Register session before daemon running");
            var entryValue = new EntryValue {Count = 1};
            Assert(_receiveCounts.TryAdd(channel, entryValue));
            Logger.LogInformation($"Channel registered: {channel.RemoteAddress}");
        }

        public virtual void UnregisterChannel(IChannel channel)
        {
            Assert(IsRunning, "Register session before daemon running");
            bool res = _receiveCounts.TryRemove(channel, out _);
            Logger.LogInformation($"Channel unregistered: {channel.RemoteAddress}, exist before remove: {res}");
        }

        public virtual void Start()
        {
            _receiveCounts.Clear();
            CancelSrc = new CancellationTokenSource();
            var taskFac = new TaskFactory(
                CancelSrc.Token,
                TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None,
                TaskScheduler.Default);
            Daemon    = taskFac.StartNew(DaemonRun);
            IsRunning = true;
            Logger.LogInformation("Timeout Daemon started");
        }

        protected virtual void DaemonRun()
        {
            while (!CancelSrc.Token.IsCancellationRequested)
            {
                Logger.LogDebug("Timeout Daemon fell asleep");
                try
                {
                    Task.Delay(_waitTime, CancelSrc.Token).Wait();
                }
                catch
                {
                    Logger.LogInformation("Timeout Daemon waked up by cancellation");
                }

                Logger.LogDebug("Timeout Daemon waked up");
                ClientsToBeRemoved = new List<KeyValuePair<IChannel, EntryValue>>();
                foreach (var entry in _receiveCounts)
                {
                    lock (entry.Value)
                    {
                        if (entry.Value.Count == 0)
                        {
                            ClientsToBeRemoved.Add(entry);
                            Assert(_receiveCounts.TryRemove(entry.Key, out EntryValue _));
                            Logger.LogInformation(
                                $"Channel {entry.Key.RemoteAddress} timeout and removed, client enqueued to be removed: {entry.Key}");
                        }
                        else
                        {
                            entry.Value.Count = 0;
                        }
                    }
                }

                foreach (var entry in ClientsToBeRemoved)
                {
                    Logger.LogDebug($"Calling upper level to dispose channel: {entry.Key}");
                    ClientTimeout?.Invoke(this, new ClientTimeoutEventArgs(entry.Key));
                }

                ClientsToBeRemoved.Clear();
            }
        }

        public virtual void Stop()
        {
            Logger.LogInformation("Timeout Daemon cancelled");
            CancelSrc.Cancel();
            Daemon.Wait();
            IsRunning = false;
            Logger.LogInformation("Timeout Daemon stopped");
        }

        public void Dispose()
        {
            if (IsRunning)
            {
                Logger.LogWarning("Dispose before stop");
                Stop();
            }

            CancelSrc.Dispose();
        }
    }

    public class EntryValue
    {
        public int Count { get; set; }
    }
}