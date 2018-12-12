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
// SimCivil - SimCivil.Rpc - HttpRequestHandler.cs
// Create Date: 2018/10/21
// Update Date: 2018/12/11

using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace SimCivil.Rpc
{
    class HttpRequestHandler : ChannelHandlerAdapter
    {
        private bool _isWebSocket;

        /// <summary>
        /// 记录之前未处理的buffer用来解决粘包的问题
        /// </summary>
        private IByteBuffer _lastReadBuffer;

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            while (true)
            {
                if (_isWebSocket)
                {
                    IByteBuffer buffer;
                    _lastReadBuffer?.ResetReaderIndex();
                    if (_lastReadBuffer != null && _lastReadBuffer.ReadableBytes != 0)
                    {
                        buffer = ByteBufferUtil.DefaultAllocator.HeapBuffer(
                            _lastReadBuffer.ReadableBytes + ((IByteBuffer) message).ReadableBytes);
                        buffer.WriteBytes(_lastReadBuffer);
                        buffer.WriteBytes((IByteBuffer) message);
                        _lastReadBuffer = buffer;
                    }
                    else
                    {
                        buffer = (IByteBuffer) message;
                        _lastReadBuffer = buffer;
                    }

                    if (buffer.ReadableBytes < 2) return;

                    IByteBuffer bufferCopy = ByteBufferUtil.DefaultAllocator.HeapBuffer(buffer.Capacity);
                    buffer.ReadBytes(bufferCopy, 2);
                    if ((bufferCopy.GetByte(0) & 8) == 8)
                    {
                        //操作码位8表示断开连接
                        context.CloseAsync();

                        return;
                    }

                    byte[] maskKey = {0, 0, 0, 0};
                    bool masked = false;
                    byte lenMark = bufferCopy.GetByte(1);
                    if (lenMark >= 128)
                    {
                        masked = true;
                        lenMark -= 128;
                    }

                    int offset = 0;
                    int len = 0;
                    if (lenMark <= 125)
                    {
                        offset = 2;
                        len = lenMark;
                    }
                    else if (lenMark == 126)
                    {
                        offset = 4;

                        if (buffer.ReadableBytes < 2) return;

                        buffer.ReadBytes(bufferCopy, 2);
                        len = bufferCopy.GetUnsignedShort(2);
                    }
                    else if (lenMark == 127)
                    {
                        offset = 10;

                        if (buffer.ReadableBytes < 8) return;

                        buffer.ReadBytes(bufferCopy, 8);
                        len = (int) bufferCopy.GetLong(2);
                    }

                    if (masked)
                    {
                        if (buffer.ReadableBytes < 4) return;

                        buffer.ReadBytes(bufferCopy, 4);
                        for (int i = 0; i < 4; i++)
                        {
                            maskKey[i] = bufferCopy.GetByte(offset + i);
                        }

                        offset += 4;
                    }

                    if (buffer.ReadableBytes < len) return;

                    buffer.ReadBytes(bufferCopy, len);
                    IByteBuffer output = ByteBufferUtil.DefaultAllocator.HeapBuffer(len);
                    for (int i = 0; i < len; i++)
                    {
                        output.WriteByte(bufferCopy.GetByte(offset + i) ^ maskKey[i % 4]);
                    }

                    _lastReadBuffer.MarkReaderIndex();
                    base.ChannelRead(context, output);

                    if (_lastReadBuffer.ReadableBytes <= 0) return;

                    _lastReadBuffer = null;
                    message = buffer;

                    continue;
                }

                try
                {
                    var buffer = (IByteBuffer) message;
                    IByteBuffer bufferCopy = buffer.Copy();
                    var bytes = new byte[bufferCopy.ReadableBytes];
                    bufferCopy.ReadBytes(bytes);
                    string data = Encoding.ASCII.GetString(bytes);
                    const string requestWebSocketMark = "Sec-WebSocket-Key:";
                    int index = data.IndexOf(requestWebSocketMark, StringComparison.Ordinal);
                    if (index < 0)
                    {
                        throw new Exception();
                    }

                    data = data.Substring(index + requestWebSocketMark.Length + 1);
                    var key = new StringBuilder();
                    foreach (char t in data)
                    {
                        if (IsBase64Char(t))
                        {
                            key.Append(t);
                        }
                        else
                        {
                            break;
                        }
                    }

                    key.Append("258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
                    SHA1 sha1 = new SHA1CryptoServiceProvider();
                    data = Convert.ToBase64String(sha1.ComputeHash(Encoding.UTF8.GetBytes(key.ToString())));
                    sha1.Dispose();
                    StringBuilder ret = new StringBuilder();
                    ret.Append("HTTP/1.1 101 Switching Protocols\r\nConnection: Upgrade\r\nSec-WebSocket-Accept: ");
                    ret.Append(data);
                    ret.Append("\r\nUpgrade: websocket\r\n\r\n");
                    IByteBuffer output = ByteBufferUtil.DefaultAllocator.HeapBuffer();
                    output.WriteBytes(Encoding.UTF8.GetBytes(ret.ToString()));
                    context.WriteAndFlushAsync(output);

                    _isWebSocket = true;
                }
                catch
                {
                    base.ChannelRead(context, message);
                }
                finally
                {
                    if (_isWebSocket)
                    {
                        context.Channel.Pipeline.Remove<LengthFieldBasedFrameDecoder>();
                        context.Channel.Pipeline.Remove<LengthFieldPrepender>();
                    }
                    else
                    {
                        context.Channel.Pipeline.Remove(this);
                    }
                }

                break;
            }
        }


        public override Task WriteAsync(IChannelHandlerContext context, object message)
        {
            if (_isWebSocket == false)
            {
                return base.WriteAsync(context, message);
            }

            IByteBuffer buffer = (IByteBuffer) message;
            IByteBuffer output = ByteBufferUtil.DefaultAllocator.HeapBuffer();
            //1000 0010，（0010表示这一帧是binary frame）
            output.WriteByte(130);
            //这里按照定义来讲最长应该是64位表示的长度，也就是应该用ulong表示。但是我并不认为一个包长度能超过Int32..
            int len = buffer.ReadableBytes;
            if (len <= 125)
            {
                output.WriteByte(len);
            }
            else if (len < 65536)
            {
                output.WriteByte(126);
                output.WriteByte((len & 0xff00) >> 8);
                output.WriteByte(len & 0xff);
            }
            else
            {
                output.WriteByte(127);
                output.WriteByte(0);
                output.WriteByte(0);
                output.WriteByte(0);
                output.WriteByte(0);
                output.WriteByte((int) ((uint) len & 0xff000000) >> 24);
                output.WriteByte((len & 0xff0000) >> 16);
                output.WriteByte((len & 0xff00) >> 8);
                output.WriteByte(len & 0xff);
            }

            output.WriteBytes(buffer);

            return context.WriteAndFlushAsync(output);
        }

        private static bool IsBase64Char(char c)
            => (c <= 'Z' && c >= 'A') || (c <= 'z' && c >= 'a') || (c <= '9' && c >= '0') || c == '+' || c == '/' ||
               c == '=';
    }
}