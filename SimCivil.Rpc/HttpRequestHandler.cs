using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using DotNetty.Buffers;
using System.Threading.Tasks;
using DotNetty.Codecs;

namespace SimCivil.Rpc
{
    class HttpRequestHandler : ChannelHandlerAdapter
    {
        public bool isWebSocket = false;
        public LengthFieldPrepender lengthFieldPrepender;
        public LengthFieldBasedFrameDecoder lengthFieldBasedFrameDecoder;
        /// <summary>
        /// 记录之前未处理的buffer用来解决粘包的问题
        /// </summary>
        private DotNetty.Buffers.IByteBuffer lastReadBuffer;
        public HttpRequestHandler(LengthFieldPrepender lengthFieldPrepender, LengthFieldBasedFrameDecoder lengthFieldBasedFrameDecoder)
        {
            this.lengthFieldPrepender = lengthFieldPrepender;
            this.lengthFieldBasedFrameDecoder = lengthFieldBasedFrameDecoder;
        }
        public override void ChannelActive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
        }
        
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (isWebSocket)
            {
                DotNetty.Buffers.IByteBuffer buffer;
                if (lastReadBuffer != null) lastReadBuffer.ResetReaderIndex();
                if (lastReadBuffer != null && lastReadBuffer.ReadableBytes != 0)
                {
                    buffer = ByteBufferUtil.DefaultAllocator.HeapBuffer(lastReadBuffer.ReadableBytes + ((DotNetty.Buffers.IByteBuffer)message).ReadableBytes);
                    buffer.WriteBytes(lastReadBuffer);
                    buffer.WriteBytes((DotNetty.Buffers.IByteBuffer)message);
                    lastReadBuffer = buffer;
                }
                else
                {
                    buffer = (DotNetty.Buffers.IByteBuffer)message;
                    lastReadBuffer = buffer;
                }
                if (buffer.ReadableBytes < 2) return;
                DotNetty.Buffers.IByteBuffer bufferCopy = ByteBufferUtil.DefaultAllocator.HeapBuffer(buffer.Capacity);
                buffer.ReadBytes(bufferCopy, 2);
                if((bufferCopy.GetByte(0) & 8) == 8)
                {
                    //操作码位8表示断开连接
                    context.CloseAsync();
                    return;
                }
                byte[] maskKey = new byte[4] { 0, 0, 0, 0 };
                bool masked = false;
                byte lenMark = bufferCopy.GetByte(1);
                if (lenMark >= 128)
                {
                    masked = true;
                    lenMark -= 128;
                }
                int offset = 0;
                int len = 0;
                if(lenMark <= 125)
                {
                    offset = 2;
                    len = lenMark;
                }
                else if(lenMark == 126)
                {
                    offset = 4;
                    if (buffer.ReadableBytes < 2) return;
                    buffer.ReadBytes(bufferCopy, 2);
                    len = bufferCopy.GetUnsignedShort(2);
                }
                else if(lenMark == 127)
                {
                    offset = 10;
                    if (buffer.ReadableBytes < 8) return;
                    buffer.ReadBytes(bufferCopy, 8);
                    len = (int)bufferCopy.GetLong(2);
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
                lastReadBuffer.MarkReaderIndex();
                base.ChannelRead(context, output);
                if(lastReadBuffer.ReadableBytes > 0)
                {
                    lastReadBuffer = null;
                    ChannelRead(context, buffer);
                }
                return;
            }
            try
            {
                DotNetty.Buffers.IByteBuffer buffer = (DotNetty.Buffers.IByteBuffer)message;
                DotNetty.Buffers.IByteBuffer bufferCopy = buffer.Copy();
                byte[] bytes = new byte[bufferCopy.ReadableBytes];
                bufferCopy.ReadBytes(bytes);
                string data = System.Text.Encoding.ASCII.GetString(bytes);
                string requestWebSocketMark = "Sec-WebSocket-Key:";
                int index = data.IndexOf(requestWebSocketMark);
                if (index < 0)
                {
                    throw new Exception();
                }
                data = data.Substring(index + requestWebSocketMark.Length + 1);
                StringBuilder key = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    if (IsBase64Char(data[i]))
                    {
                        key.Append(data[i]);
                    }
                    else
                    {
                        break;
                    }
                }
                key.Append("258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                data = Convert.ToBase64String(sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key.ToString())));
                sha1.Dispose();
                StringBuilder ret = new StringBuilder();
                ret.Append("HTTP/1.1 101 Switching Protocols\r\nConnection: Upgrade\r\nSec-WebSocket-Accept: ");
                ret.Append(data);
                ret.Append("\r\nUpgrade: websocket\r\n\r\n");
                IByteBuffer output = ByteBufferUtil.DefaultAllocator.HeapBuffer();
                output.WriteBytes(System.Text.Encoding.UTF8.GetBytes(ret.ToString()));
                context.WriteAndFlushAsync(output);

                isWebSocket = true;
            }
            catch
            {
                base.ChannelRead(context, message);
            }
            finally
            {
                if (isWebSocket == false)
                {
                    context.Channel.Pipeline.Remove(this);
                }
                else
                {
                    context.Channel.Pipeline.Remove(lengthFieldPrepender);
                    context.Channel.Pipeline.Remove(lengthFieldBasedFrameDecoder);
                }
            }
        }



        public override Task WriteAsync(IChannelHandlerContext context, object message)
        {
            if (isWebSocket == false)
            {
                return base.WriteAsync(context, message);
            }
            else
            {
                DotNetty.Buffers.IByteBuffer buffer = (DotNetty.Buffers.IByteBuffer)message;
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
                    output.WriteByte((int)((uint)len & 0xff000000) >> 24);
                    output.WriteByte((len & 0xff0000) >> 16);
                    output.WriteByte((len & 0xff00) >> 8);
                    output.WriteByte(len & 0xff);
                }
                output.WriteBytes(buffer);
                return context.WriteAndFlushAsync(output);
            }
        }

        private bool IsBase64Char(char c)
        {
            return (c <= 'Z' && c >= 'A') || (c <= 'z' && c >= 'a') || (c <= '9' && c >= '0') || c == '+' || c == '/' || c == '=';
        }
    }
}
