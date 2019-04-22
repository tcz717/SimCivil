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
    public class SimpleTimeoutDeamon : ITimeoutDeamon
    {
        private static readonly int WaitTime = 5000;
        public event EventHandler<ClientTimeoutEventArgs> ClientTimeout;
        protected List<KeyValuePair<IChannel, EntryValue>> clientsToBeRemoved = new List<KeyValuePair<IChannel, EntryValue>>();
        protected readonly RpcServer server;
        protected readonly ILogger log;
        protected Task deamon;
        protected CancellationTokenSource cancelSrc;
        public bool IsRunning { get; private set; } = false;

        private readonly ConcurrentDictionary<IChannel, EntryValue> receiveCounts = new ConcurrentDictionary<IChannel, EntryValue>();

        public SimpleTimeoutDeamon(RpcServer server, ILogger logger)
        {
            this.server = server ?? throw new ArgumentNullException(nameof(server));
            this.log = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool NotifyPacketReceived(IChannel channel, RpcRequest request)
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

        public void RegisterChannel(IChannel channel)
        {
            Assert(IsRunning, "Register session before deamon running");
            Assert(receiveCounts.TryAdd(channel, new EntryValue()));
            log.LogInformation($"Channel registered: {channel}");
        }

        public void UnregisterChannel(IChannel channel)
        {
            Assert(IsRunning, "Register session before deamon running");
            var res = receiveCounts.TryRemove(channel, out _);
            log.LogInformation($"Channel unregistered: {channel}, exist before remove: {res}");
        }

        public void Start()
        {
            log.LogInformation("Timeout Deamon starting");
            receiveCounts.Clear();
            cancelSrc = new CancellationTokenSource();
            log.LogInformation("Timeout Deamon starting 1");
            TaskFactory taskFac = new TaskFactory(cancelSrc.Token, TaskCreationOptions.LongRunning, TaskContinuationOptions.None, TaskScheduler.Default);
            log.LogInformation("Timeout Deamon starting 2");
            deamon = Task.Run(DeamonRun);
            IsRunning = true;
            log.LogInformation("Timeout Deamon started");
        }

        protected void DeamonRun()
        {
            while (!cancelSrc.Token.IsCancellationRequested)
            {
                log.LogDebug("Timeout Deamon waked up");
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

                log.LogDebug("Timeout Deamon fell asleep");
                try
                {
                    Task.Delay(WaitTime, cancelSrc.Token).Wait();
                }
                catch
                {
                    log.LogInformation("Timeout Deamon waked up by cancellation");
                }
            }
        }

        public void Stop()
        {
            log.LogInformation("Timeout Deamon cancelled");
            cancelSrc.Cancel();
            deamon.Wait();
            IsRunning = false;
            log.LogInformation("Timeout Deamon stopped");
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
