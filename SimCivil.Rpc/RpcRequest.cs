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
// SimCivil - SimCivil.Rpc - RpcRequest.cs
// Create Date: 2017/12/31
// Update Date: 2018/01/01

using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SimCivil.Rpc
{
    public class RpcRequest
    {
        private RpcResponse _response;
        private readonly AutoResetEvent _waitEvent;
        private readonly TaskCompletionSource<RpcResponse> _responseSource;
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public object[] Arguments { get; set; }
        public long Sequence { get; set; }

        public RpcRequest() { }

        public RpcRequest(long sequence, MethodInfo method, object[] arguments)
        {
            ServiceName = method.DeclaringType.FullName;
            MethodName = method.Name;
            Arguments = arguments;
            Sequence = sequence;
            _waitEvent = new AutoResetEvent(false);
            _responseSource=new TaskCompletionSource<RpcResponse>();
        }

        public void PutResponse(RpcResponse response)
        {
            if (_responseSource == null)
            {
                throw new NotSupportedException();
            }
            _responseSource.SetResult(response ?? throw new ArgumentNullException(nameof(response)));
        }

        public RpcResponse WaitResponse(int millisecondsTimeout)
        {
            if (!_responseSource.Task.Wait(millisecondsTimeout))
            {
                throw new TimeoutException();
            }

            return _responseSource.Task.Result;
        }

        public async Task<RpcResponse> WaitResponseAsync(int millisecondsTimeout)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource(millisecondsTimeout);
            tokenSource.Token.Register(() => _responseSource.TrySetCanceled());

            return await _responseSource.Task;
        }
    }
}