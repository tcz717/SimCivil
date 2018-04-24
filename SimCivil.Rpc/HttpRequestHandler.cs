using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using DotNetty.Buffers;

namespace SimCivil.Rpc
{
    class HttpRequestHandler : ChannelHandlerAdapter
    {
        public bool isWebSocket = false;
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine("HttpRequestHandler ChannelActive");
            base.ChannelActive(context);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (isWebSocket)
            {
                DotNetty.Buffers.IByteBuffer buffer = (DotNetty.Buffers.IByteBuffer)message;
                DotNetty.Buffers.IByteBuffer bufferCopy = buffer.Copy();
                byte[] bytes = new byte[bufferCopy.ReadableBytes];
                bufferCopy.ReadBytes(bytes);
                byte[] maskKey = new byte[4] { 0, 0, 0, 0 };
                bool masked = false;
                if (bytes[1] >= 128)
                {
                    masked = true;
                    bytes[1] -= 128;
                }
                int offset = 0;
                if(bytes[1] <= 125)
                {
                    offset = 2;
                }
                else if(bytes[1] == 126)
                {
                    offset = 4;
                }
                else if(bytes[1] == 127)
                {
                    offset = 10;
                }
                if (masked)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        maskKey[i] = bytes[offset + i];
                    }
                    offset += 4;
                }
                IByteBuffer output = ByteBufferUtil.DefaultAllocator.HeapBuffer();
                for (int i = 0; i < bytes.Length - offset; i++)
                {
                    output.WriteByte(bytes[offset + i] ^ maskKey[i % 4]);
                }
                
                Console.WriteLine(output);
                base.ChannelRead(context, output);
                return;
            }
            Console.WriteLine(message.ToString());
            try
            {
                DotNetty.Buffers.IByteBuffer buffer = (DotNetty.Buffers.IByteBuffer)message;
                DotNetty.Buffers.IByteBuffer bufferCopy = buffer.Copy();
                byte[] bytes = new byte[bufferCopy.ReadableBytes];
                bufferCopy.ReadBytes(bytes);
                for(int i = 0; i < bytes.Length; i++)
                {
                    Console.Write(bytes[i] + " ");
                }
                Console.WriteLine();
                string data = System.Text.Encoding.ASCII.GetString(bytes);
                Console.WriteLine(data);
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
                Console.WriteLine(key.ToString());
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                data = Convert.ToBase64String(sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(key.ToString())));
                sha1.Dispose();
                StringBuilder ret = new StringBuilder();
                ret.Append("HTTP/1.1 101 Switching Protocols\r\nConnection: Upgrade\r\nSec-WebSocket-Accept: ");
                ret.Append(data);
                ret.Append("\r\nUpgrade: websocket\r\n\r\n");
                Console.WriteLine(ret.ToString());
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
            }
        }

        private bool IsBase64Char(char c)
        {
            return (c <= 'Z' && c >= 'A') || (c <= 'z' && c >= 'a') || (c <= '9' && c >= '0') || c == '+' || c == '/' || c == '=';
        }
    }
}
