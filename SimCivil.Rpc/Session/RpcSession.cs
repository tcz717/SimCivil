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
// SimCivil - SimCivil.Rpc - RpcSession.cs
// Create Date: 2018/01/02
// Update Date: 2018/01/02

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SimCivil.Rpc.Session
{
    public class LocalRpcSession : Dictionary<string, object>, IRpcSession
    {
        public LocalRpcSession() { }
        public IPEndPoint RemoteEndPoint { get; set; }
        public event EventHandler Exiting;
        public event EventHandler<EventArgs<EndPoint>> Entering;

        public virtual void OnExiting()
        {
            Exiting?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnEntering(EndPoint endPoint)
        {
            Entering?.Invoke(this, new EventArgs<EndPoint>(endPoint));
        }
    }

    public interface IRpcSession : IDictionary<string, object>
    {
        IPEndPoint RemoteEndPoint { get; set; }

        event EventHandler Exiting;
        event EventHandler<EventArgs<EndPoint>> Entering;
        void OnExiting();
        void OnEntering(EndPoint endPoint);
    }
}