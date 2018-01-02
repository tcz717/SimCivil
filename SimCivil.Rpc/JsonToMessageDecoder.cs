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
// SimCivil - SimCivil.Rpc - JsonToMessageDecoder.cs
// Create Date: 2017/12/31
// Update Date: 2018/01/01

using System;
using System.Collections.Generic;
using System.Text;

using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

using Newtonsoft.Json;

namespace SimCivil.Rpc
{
    public class JsonToMessageDecoder<T> : MessageToMessageDecoder<IByteBuffer> where T : class
    {
        public static Action<string> TestHook;

        protected override void Decode(IChannelHandlerContext context, IByteBuffer message, List<object> output)
        {
            string jsonStr = message.ToString(Encoding.UTF8);
            TestHook?.Invoke(jsonStr);
            output.Add(JsonConvert.DeserializeObject<T>(jsonStr));
        }
    }
}