using DotNetty.Transport.Channels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;

namespace SimCivil.Rpc.Timeout
{
    /// <summary>
    /// One clock hand algorithm timeout daemon
    /// </summary>
    public class SimpleTimeoutDaemon : ITimeoutDaemon
    {
        private readonly int waitTime;
        public event EventHandler<ClientTimeoutEventArgs> ClientTimeout;
        protected List<KeyValuePair<IChannel, EntryValue>> clientsToBeRemoved = new List<KeyValuePair<IChannel, EntryValue>>();
        protected readonly RpcServer server;
        protected readonly ILogger log;
        protected Task daemon;
        protected CancellationTokenSource cancelSrc;
        public bool IsRunning { get; private set; } = false;

        private readonly ConcurrentDictionary<IChannel, EntryValue> receiveCounts = new ConcurrentDictionary<IChannel, EntryValue>();

        public SimpleTimeoutDaemon(RpcServer server, ILogger logger, int waitTime)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.log = logger ?? throw new ArgumentNullException(nameof(logger));
            this.waitTime = waitTime;
        }

        public virtual bool NotifyPacketReceived(IChannel channel, RpcRequest request)
        {
            if (!receiveCounts.TryGetValue(channel, out EntryValue cnt))
            {
                log.LogInformation($"Notify a timeout channel: {channel}");
                return false;
            }

            lock (cnt)
            {
                if (!receiveCounts.TryGetValue(channel, out EntryValue sameCnt))
                {
                    log.LogInformation("Channel found but after lock it is timeout");
                    return false;
                }
                cnt.Count++;
                log.LogDebug($"Session cnt increased to {cnt.Count}, channel: {channel}");
            }
            return true;
        }

        public virtual void RegisterChannel(IChannel channel)
        {
            Assert(IsRunning, "Register session before daemon running");
            Assert(receiveCounts.TryAdd(channel, new EntryValue()));
            log.LogInformation($"Channel registered: {channel}");
        }

        public virtual void UnregisterChannel(IChannel channel)
        {
            Assert(IsRunning, "Register session before daemon running");
            var res = receiveCounts.TryRemove(channel, out _);
            log.LogInformation($"Channel unregistered: {channel}, exist before remove: {res}");
        }

        public virtual void Start()
        {
            log.LogInformation("Timeout Daemon starting");
            receiveCounts.Clear();
            cancelSrc = new CancellationTokenSource();
            log.LogInformation("Timeout Daemon starting 1");
            TaskFactory taskFac = new TaskFactory(cancelSrc.Token, TaskCreationOptions.LongRunning, TaskContinuationOptions.None, TaskScheduler.Default);
            log.LogInformation("Timeout Daemon starting 2");
            daemon = Task.Run(DaemonRun);
            IsRunning = true;
            log.LogInformation("Timeout Daemon started");
        }

        protected virtual void DaemonRun()
        {
            while (!cancelSrc.Token.IsCancellationRequested)
            {
                log.LogDebug("Timeout Daemon waked up");
                clientsToBeRemoved = new List<KeyValuePair<IChannel, EntryValue>>();
                foreach (var entry in receiveCounts)
                {
                    lock (entry.Value)
                    {
                        if (entry.Value.Count == 0)
                        {
                            clientsToBeRemoved.Add(entry);
                            Assert(receiveCounts.TryRemove(entry.Key, out EntryValue val));
                            log.LogInformation($"Channel removed and client enqueued to be removed: {entry.Key}");
                        }
                        else
                        {
                            entry.Value.Count = 0;
                        }
                    }
                }
                foreach (var entry in clientsToBeRemoved)
                {
                    log.LogDebug($"Calling upper level to dispose channel: {entry.Key}");
                    ClientTimeout(this, new ClientTimeoutEventArgs(entry.Key));
                }
                clientsToBeRemoved.Clear();

                log.LogDebug("Timeout Daemon fell asleep");
                try
                {
                    Task.Delay(waitTime, cancelSrc.Token).Wait();
                }
                catch
                {
                    log.LogInformation("Timeout Daemon waked up by cancellation");
                }
            }
        }

        public virtual void Stop()
        {
            log.LogInformation("Timeout Daemon cancelled");
            cancelSrc.Cancel();
            daemon.Wait();
            IsRunning = false;
            log.LogInformation("Timeout Daemon stopped");
        }

        public void Dispose()
        {
            if (IsRunning)
            {
                log.LogWarning("Dispose before stop");
                Stop();
            }
        }
    }

    public class EntryValue
    {
        public int Count = 0;
    }
}
