using DotNetty.Transport.Channels;
using SimCivil.Rpc.Session;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Rpc.Timeout
{
    /// <summary>
    /// Channel and Session that timeout
    /// </summary>
    public class ClientTimeoutEventArgs : EventArgs
    {
        public IChannel Channel;
        public ClientTimeoutEventArgs(IChannel c)
        {
            Channel = c;
        }
    }
}
