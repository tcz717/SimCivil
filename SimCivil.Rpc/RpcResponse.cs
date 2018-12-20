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
// SimCivil - SimCivil.Rpc - RpcResponse.cs
// Create Date: 2018/01/02
// Update Date: 2018/01/05

using System;
using System.Text;

using Newtonsoft.Json;

using SimCivil.Rpc.Serialize;

namespace SimCivil.Rpc
{
    [JsonConverter(typeof(RpcMessageConverter))]
    public class RpcResponse
    {
        public object ReturnValue { get; set; }
        public long Sequence { get; set; }
        public string ErrorInfo { get; set; }
        public DateTime TimeStamp { get; set; }

        public RpcResponse() { }

        public RpcResponse(RpcRequest request, object returnValue)
        {
            ReturnValue = returnValue;
            Sequence = request.Sequence;
            TimeStamp = DateTime.UtcNow;
        }

        public RpcResponse(RpcRequest request, object returnValue, Exception exception) : this(request, returnValue)
        {
            ErrorInfo = exception?.ToString();
        }

        public RpcResponse(RpcRequest request, object returnValue, string errorInfo) : this(request, returnValue)
        {
            ErrorInfo = errorInfo;
        }
    }
}