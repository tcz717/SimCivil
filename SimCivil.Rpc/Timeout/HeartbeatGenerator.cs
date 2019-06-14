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
    internal class HeartbeatGenerator : IHeartbeatGenerator
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

        public HeartbeatGenerator(RpcClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Start the daemon
        /// </summary>
        public void Start()
        {
            _cancel = new CancellationTokenSource();
            var tf = new TaskFactory(
                _cancel.Token,
                TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None,
                TaskScheduler.Default);
            _runTask = tf.StartNew(Run);
            IsRunning = true;
        }

        /// <summary>
        /// Stop the daemon
        /// </summary>
        public void Stop()
        {
            _cancel.Cancel();
            _runTask.Wait();
            IsRunning = false;
        }

        /// <summary>
        /// Notify that a request has been sent
        /// </summary>
        public void NotifyPacketSent()
        {
            if (!IsRunning)
                throw new InvalidOperationException("HeartbeatGenerator is stopped");

            _sentPacket = true;
        }

        private void Run()
        {
            while (!_cancel.IsCancellationRequested)
            {
                try
                {
                    Task.Delay(_client.HeartbeatDelay, _cancel.Token).Wait();
                }
                catch
                {
                    // Log cancelled
                }

                if (!_sentPacket)
                    HeartbeatNeeded?.Invoke(this, new EventArgs());
                else
                    _sentPacket = false;
            }
        }

        public void Dispose()
        {
            if (IsRunning)
                Stop();
            _cancel.Dispose();
        }
    }
}
