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
// SimCivil - SimCivil.Rpc - RpcInterceptor.cs
// Create Date: 2018/01/02
// Update Date: 2018/01/02

using System;
using System.Text;

using Castle.DynamicProxy;

namespace SimCivil.Rpc
{
    internal class RpcInterceptor : IInterceptor
    {
        private readonly RpcClient _rpcClient;

        public RpcInterceptor(RpcClient rpcClient)
        {
            _rpcClient = rpcClient;
        }

        public void Intercept(IInvocation invocation)
        {
            RpcRequest request = new RpcRequest(_rpcClient.GetNextSequence(), invocation.Method, invocation.Arguments);
            _rpcClient.ResponseWaitlist.Add(request.Sequence, request);
            try
            {
                _rpcClient.Channel.WriteAndFlushAsync(request);
                RpcResponse response = request.WaitResponse(_rpcClient.ResponseTimeout);

                if (!string.IsNullOrEmpty(response.ErrorInfo))
                    throw new RemotingException(response.ErrorInfo, invocation.Method, invocation.Arguments);

                if (!invocation.Method.ReturnType.IsInstanceOfType(response.ReturnValue))
                {
                    if (invocation.Method.ReturnType == typeof(void) && response.ReturnValue is null)
                        return;

                    throw new InvalidCastException();
                }

                invocation.ReturnValue = response.ReturnValue;
            }
            finally
            {
                _rpcClient.ResponseWaitlist.Remove(request.Sequence);
            }
        }
    }
}