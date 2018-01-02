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
// Create Date: 2018/01/01
// Update Date: 2018/01/01

using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace SimCivil.Rpc
{
    [Serializable]
    public class RemotingException : Exception
    {
        public MethodInfo Method { get; }
        public object[] Arguments { get; }

        public RemotingException()
        {
        }

        public RemotingException(string message) : base(message)
        {
        }

        public RemotingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RemotingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RemotingException(string message, MethodInfo method, object[] arguments) 
            : base(message)
        {
            Method = method;
            Arguments = arguments;
        }
    }
}