using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections;
using SimCivil.Net.Packets;

namespace SimCivil.Net
{
    /// <summary>
    /// The base class of Packet, including Head, data, client, and send/handle methods etc.
    /// </summary>
    public abstract class Packet
    {
        /// <summary>
        /// 单个数据包最大限制
        /// </summary>
        public const int MaxSize = 4096; 

        /// <summary>
        /// Packet head storing ID, type, and body length
        /// </summary>
        protected Head head;

        /// <summary>
        /// The client indicating where to send to or received from
        /// </summary>
        protected IServerConnection client;

        /// <summary>
        /// The dictionary storing data, consist of a string and a value
        /// </summary>
        public Hashtable Data { get; set; }
        /// <summary>
        /// Packet head storing ID, type, and body length
        /// </summary>
        public Head Head { get { return head; } set { head = value; } }
        /// <summary>
        /// The client indicating where to send to or received from
        /// </summary>
        public IServerConnection Client { get { return client; } set { client = value; } }

        /// <summary>
        /// Send time.
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return (DateTime)Data[nameof(Timestamp)];
            }
            set
            {
                Data[nameof(Timestamp)] = value;
            }
        }

        /// <summary>
        /// Construct a Packet, type will be automatically added into head
        /// </summary>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="client">client indicating where to send to or received from</param>
        public Packet(Hashtable data=null, IServerConnection client = null)
        {
            Data = data ?? new Hashtable();
            this.client = client;
            head = default(Head);
            head.type = GetType().GetTypeInfo().GetCustomAttribute<PacketTypeAttribute>().PacketType;
        }

        /// <summary>
        /// Give order to send packet immediately. 
        /// Note: This is a method especially for server, please do NOT use it directly!
        /// It is recommended to enqueue packets into PacketSendQueue for sending
        /// </summary>
        public virtual void Send()
        {
            client.SendPacket(this);
        }

        /// <summary>
        /// The method executed after clients received and pushed in the PacketReadQueue
        /// </summary>
        public virtual void Handle() { }

        /// <summary>
        /// If the packet need futher procedure, this method will be called when response received.
        /// </summary>
        /// <param name="packet">Pesponse packet.</param>
        [Obsolete]
        public virtual void ResponseCallback(Packet packet) { }
        
        /// <summary>
        /// Update ID and length stored in head and convert Packet, including head, to bytes
        /// </summary>
        /// <param name="packetID">the ID of packet for assembling head</param>
        /// <returns>bytes converted from Packet</returns>
        public virtual byte[] ToBytes(int packetID)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Data));

            head.length = dataBytes.Length;
            head.packetID = packetID;

            return head.ToBytes().Concat(dataBytes).ToArray();
        }

        /// <summary>
        /// Verify this packet's receiving correctness.
        /// </summary>
        /// <returns></returns>
        public virtual bool Verify(out string errorDesc)
        {
            bool result = true;
            result &= Enum.GetNames(typeof(PacketType)).Contains(head.type.ToString());
            result &= head.length > 0;
            result &= Data != null;
            if (!result)
                errorDesc = "Packet format invaild";
            result &= !PacketFactory.PacketAttributes[head.type].LoginRequired || Client.ContextPlayer != null;
            if (result)
                errorDesc = "";
            else
                errorDesc = "Login required";
            return result;
        }

        public void Reply(ResponsePacket response)
        {
            response.Client = client;
            response.RefPacketID = head.packetID;
            client.SendPacket(response);
        }

        public void ReplyError(int errorCode = 0, string desc = "error occured") =>
            Reply(new ErrorResponse(errorCode, desc) { Client = client });
        public void ReplyOk(string desc = "request ok") =>
            Reply(new OkResponse(desc) { Client = client });
    }
    
    /// <summary>
    /// A class describes Packet Head
    /// </summary>
    public struct Head
    {
        /// <summary>
        /// Fixed head length
        /// </summary>
        public const int HeadLength = 12;

        /// <summary>
        /// Packet ID
        /// </summary>
        public int packetID;

        /// <summary>
        /// Packet type
        /// </summary>
        public PacketType type;

        /// <summary>
        /// Length of body
        /// </summary>
        public int length;

        /// <summary>
        /// Construct a Packet Head
        /// </summary>
        /// <param name="type">Packet type</param>
        public Head(PacketType type) : this(0, type, 0) { }

        /// <summary>
        /// Construct a Packet Head
        /// </summary>
        /// <param name="packageID">Packet ID</param>
        /// <param name="type">Packet type</param>
        public Head(int packageID, PacketType type) : this(packageID, type, 0) { }

        /// <summary>
        ///  Construct a Packet Head
        /// </summary>
        /// <param name="packageID">Packet ID</param>
        /// <param name="type">Packet type</param>
        /// <param name="length">length of body</param>
        public Head(int packageID, PacketType type, int length)
        {
            this.packetID = packageID;
            this.length = length;
            this.type = type;
        }

        /// <summary>
        /// Build head from bytes
        /// </summary>
        /// <param name="buffer">buffer to build head</param>
        /// <returns>a brand new head</returns>
        public static Head FromBytes(byte[] buffer)
        {
            int packageID = BitConverter.ToInt32(buffer, 0);
            PacketType type = (PacketType)BitConverter.ToInt32(buffer, 4);
            int length = BitConverter.ToInt32(buffer, 8);

            return new Head(packageID, type, length);
        }

        /// <summary>
        /// Convert head to bytes for sending, Note: this method has been used in Head.ToBytes() for building packet bytes
        /// </summary>
        /// <returns>bytes converted from head</returns>
        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(packetID));
            bytes.AddRange(BitConverter.GetBytes((int)type));
            bytes.AddRange(BitConverter.GetBytes(length));

            return bytes.ToArray();
        }
    }
}
