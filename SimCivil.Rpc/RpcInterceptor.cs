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
// Update Date: 2018/01/05

using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Castle.DynamicProxy;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimCivil.Rpc
{
    internal class RpcInterceptor : IInterceptor
    {
        private static readonly MethodInfo AsyncReturnHandleInfo = typeof(RpcInterceptor).GetMethod(
            nameof(AsyncReturnHandle),
            BindingFlags.Instance | BindingFlags.NonPublic);

        private readonly RpcClient _rpcClient;

        public RpcInterceptor(RpcClient rpcClient)
        {
            _rpcClient = rpcClient;
        }

        public void Intercept(IInvocation invocation)
        {
            ReplaceCallback(invocation.Arguments);
            RpcRequest request = new RpcRequest(_rpcClient.GetNextSequence(), invocation.Method, invocation.Arguments);
            _rpcClient.ResponseWaitlist.Add(request.Sequence, request);
            _rpcClient.Channel.WriteAndFlushAsync(request);
            Type returnType = invocation.Method.ReturnType;
            _rpcClient.NotifyPacketSent();
            switch (returnType.GetDelegateType())
            {
                case MethodType.Synchronous:
                    try
                    {
                        RpcResponse response = request.WaitResponse(_rpcClient.ResponseTimeout);
                        CheckReturn(invocation, response);
                        invocation.ReturnValue = response.ReturnValue;
                    }
                    finally
                    {
                        _rpcClient.ResponseWaitlist.Remove(request.Sequence);
                    }

                    return;
                case MethodType.AsyncAction:
                    invocation.ReturnValue = request.WaitResponseAsync(_rpcClient.ResponseTimeout)
                        .ContinueWith(
                            t =>
                            {
                                try
                                {
                                    CheckTaskReturn(invocation, t.Result);

                                    return t.Result;
                                }
                                finally
                                {
                                    _rpcClient.ResponseWaitlist.Remove(request.Sequence);
                                }
                            },
                            TaskContinuationOptions.AttachedToParent);

                    return;
                case MethodType.AsyncFunction:
                    invocation.ReturnValue = AsyncReturnHandleInfo.MakeGenericMethod(returnType.GenericTypeArguments)
                        .Invoke(
                            this,
                            new object[] {invocation, request.WaitResponseAsync(_rpcClient.ResponseTimeout), request});

                    return;
            }
        }

        /// <summary>Replaces the callbacks with ids.</summary>
        /// <param name="arguments">The arguments.</param>
        /// <exception cref="NotSupportedException"></exception>
        private void ReplaceCallback(object[] arguments)
        {
            for (int i = 0; i < arguments.Length; i++)
            {
                if (!(arguments[i] is Delegate))
                {
                    continue;
                }

                Delegate d = (Delegate) arguments[i];

                if (d.Method.ReturnType != typeof(void))
                    throw new NotSupportedException();
                
                arguments[i] = _rpcClient.AttachCallback(d);
            }
        }

        private async Task<T> AsyncReturnHandle<T>(IInvocation invocation, Task<RpcResponse> resp, RpcRequest request)
        {
            try
            {
                RpcResponse asyncResponse = await resp;
                CheckTaskReturn(invocation, asyncResponse);
                return (T) asyncResponse.ReturnValue;
            }
            finally
            {
                _rpcClient.ResponseWaitlist.Remove(request.Sequence);
            }
        }

        /// <summary>
        /// Fixes ugly if return value is JObject.
        /// </summary>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="rpcResponse">The RPC response.</param>
        private static void FixJObject(Type returnType, RpcResponse rpcResponse)
        {
            if (rpcResponse.ReturnValue is JToken token)
            {
                rpcResponse.ReturnValue = token.ToObject(
                    returnType,
                    JsonSerializer.Create(UtilHelper.RpcJsonSerializerSettings));
            }
        }

        /// <summary>Checks if the return is successful and fixes response.</summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="response">The response.</param>
        /// <exception cref="RemotingException"></exception>
        /// <exception cref="InvalidCastException">Return type is {returnType}</exception>
        private static void CheckReturn(IInvocation invocation, RpcResponse response)
        {
            if (!string.IsNullOrEmpty(response.ErrorInfo))
                throw new RemotingException(response.ErrorInfo, invocation.Method, invocation.Arguments);

            Type returnType = invocation.Method.ReturnType;

            FixJObject(returnType, response);

            if (returnType.IsInstanceOfType(response.ReturnValue)) return;
            if (returnType == typeof(void) && response.ReturnValue is null)
                return;

            throw new InvalidCastException($"Return type is {returnType}");
        }

        /// <summary>Checks if the return is successful and fixes response.</summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="response">The response.</param>
        /// <exception cref="RemotingException"></exception>
        /// <exception cref="InvalidCastException">Return type is {response.ReturnValue?.GetType()}</exception>
        private static void CheckTaskReturn(IInvocation invocation, RpcResponse response)
        {
            if (!string.IsNullOrEmpty(response.ErrorInfo))
                throw new RemotingException(response.ErrorInfo, invocation.Method, invocation.Arguments);

            if (invocation.Method.ReturnType == typeof(Task))
                return;

            Type returnType = invocation.Method.ReturnType.GenericTypeArguments[0];
            FixJObject(returnType, response);
            if (!returnType.IsInstanceOfType(response.ReturnValue))
            {
                throw new InvalidCastException($"Return type is {response.ReturnValue?.GetType()}");
            }
        }
    }
}