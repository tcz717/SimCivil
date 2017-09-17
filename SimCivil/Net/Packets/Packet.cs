using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;

namespace SimCivil.Net.Packets
{
    /// <summary>
    /// The base class of Packet, including Head, data, client, and send/handle methods etc.
    /// </summary>
    public abstract class Packet
    {
        private Head _packetHead;

        /// <summary>
        /// 单个数据包最大限制
        /// </summary>
        public const int MaxSize = 4096;

        /// <summary>
        /// The dictionary storing data, consist of a string and a value
        /// </summary>
        public Hashtable Data { get; set; }

        /// <summary>
        /// Packet head storing ID, type, and body length
        /// </summary>
        public Head PacketHead
        {
            get => _packetHead;
            set => _packetHead = value;
        }

        /// <summary>
        /// The client indicating where to send to or received from
        /// </summary>
        public IServerConnection Client { get; set; }

        /// <summary>
        /// Send time.
        /// </summary>
        public DateTime Timestamp
        {
            get => GetDataProperty<DateTime>();
            set => SetDataProperty(value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is replied.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is replied; otherwise, <c>false</c>.
        /// </value>
        public bool IsReplied { get; protected set; }

        /// <summary>
        /// Construct a Packet, type will be automatically added into head
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data">dictionary storing data, consist of a string and a value</param>
        /// <param name="client">client indicating where to send to or received from</param>
        protected Packet(PacketType type = PacketType.Empty, Hashtable data = null, IServerConnection client = null)
        {
            Data = data ?? new Hashtable();
            Client = client;
            PacketHead = default(Head);
            UpdateType(type);
        }

        private void UpdateType(PacketType type)
        {
            Head packetHead = PacketHead;
            packetHead.Type = type == PacketType.Empty
                ? GetType().GetTypeInfo().GetCustomAttribute<PacketTypeAttribute>().PacketType
                : type;
            PacketHead = packetHead;
        }

        /// <summary>
        /// Give order to send packet immediately. 
        /// Note: This is a method especially for server, please do NOT use it directly!
        /// It is recommended to enqueue packets into PacketSendQueue for sending
        /// </summary>
        public virtual void Send()
        {
            Timestamp = DateTime.Now;
            Client.SendPacket(this);
        }

        /// <summary>
        /// The method executed after clients received and pushed in the PacketReadQueue
        /// </summary>
        public virtual void Handle()
        {
        }

        /// <summary>
        /// If the packet need further procedure, this method will be called when response received.
        /// </summary>
        /// <param name="packet">Response packet.</param>
        [Obsolete]
        public virtual void ResponseCallback(Packet packet)
        {
        }

        /// <summary>
        /// Update ID and length stored in head and convert Packet, including head, to bytes
        /// </summary>
        /// <param name="packetId">the ID of packet for assembling head</param>
        /// <returns>bytes converted from Packet</returns>
        public virtual byte[] ToBytes(int packetId)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Data));

            _packetHead.Length = dataBytes.Length;
            _packetHead.PacketId = packetId;

            return PacketHead.ToBytes().Concat(dataBytes).ToArray();
        }

        /// <summary>
        /// Verify this packet's receiving correctness.
        /// </summary>
        /// <returns></returns>
        public virtual bool Verify(out string errorDesc)
        {
            bool result = true;
            result &= Enum.GetNames(typeof(PacketType)).Contains(PacketHead.Type.ToString());
            result &= PacketHead.Length > 0;
            result &= Data != null;
            if (!result)
                errorDesc = "Packet format invalid";
            result &= !PacketFactory.PacketAttributes[PacketHead.Type].LoginRequired || Client.ContextPlayer != null;
            errorDesc = result ? "" : "Login required";
            return result;
        }

        /// <summary>
        /// Replies the specified response.
        /// </summary>
        /// <param name="response">The response.</param>
        public void Reply(ResponsePacket response)
        {
            if (IsReplied) throw new InvalidOperationException($"Packet {this} has replied");
            IsReplied = true;
            response.Client = Client;
            response.RefPacketId = PacketHead.PacketId;
            response.Send();
        }

        /// <summary>
        /// Replies the error.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="desc">The desc.</param>
        public void ReplyError(int errorCode = 0, string desc = "error occurred") =>
            Reply(new ErrorResponse(errorCode, desc) {Client = Client});

        /// <summary>
        /// Replies the ok.
        /// </summary>
        /// <param name="desc">The desc.</param>
        public void ReplyOk(string desc = "request ok") =>
            Reply(new OkResponse(true, desc) {Client = Client});

        /// <summary>
        /// Replies the deny.
        /// </summary>
        /// <param name="desc">The desc.</param>
        public void ReplyDeny(string desc = "request denied") =>
            Reply(new OkResponse(false, desc) {Client = Client});

        /// <summary>
        /// Gets the data property.
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">message - propertyName</exception>
        protected T GetDataProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("message", nameof(propertyName));
            }

            return (T) Data[propertyName];
        }

        /// <summary>
        /// Sets the data property.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <exception cref="System.ArgumentException">message - propertyName</exception>
        protected void SetDataProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("message", nameof(propertyName));
            }

            Data[propertyName] = value;
        }
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
        public int PacketId;

        /// <summary>
        /// Packet type
        /// </summary>
        public PacketType Type;

        /// <summary>
        /// Length of body
        /// </summary>
        public int Length;

        /// <summary>
        /// Construct a Packet Head
        /// </summary>
        /// <param name="type">Packet type</param>
        public Head(PacketType type) : this(0, type, 0)
        {
        }

        /// <summary>
        /// Construct a Packet Head
        /// </summary>
        /// <param name="packageId">Packet ID</param>
        /// <param name="type">Packet type</param>
        public Head(int packageId, PacketType type) : this(packageId, type, 0)
        {
        }

        /// <summary>
        /// Construct a Packet Head
        /// </summary>
        /// <param name="packageId">Packet ID</param>
        /// <param name="type">Packet type</param>
        /// <param name="length">length of body</param>
        public Head(int packageId, PacketType type, int length)
        {
            PacketId = packageId;
            Length = length;
            Type = type;
        }

        /// <summary>
        /// Build head from bytes
        /// </summary>
        /// <param name="buffer">buffer to build head</param>
        /// <returns>a brand new head</returns>
        public static Head FromBytes(byte[] buffer)
        {
            int packageId = BitConverter.ToInt32(buffer, 0);
            PacketType type = (PacketType) BitConverter.ToInt32(buffer, 4);
            int length = BitConverter.ToInt32(buffer, 8);

            return new Head(packageId, type, length);
        }

        /// <summary>
        /// Convert head to bytes for sending, Note: this method has been used in Head.ToBytes() for building packet bytes
        /// </summary>
        /// <returns>bytes converted from head</returns>
        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(PacketId));
            bytes.AddRange(BitConverter.GetBytes((int) Type));
            bytes.AddRange(BitConverter.GetBytes(Length));

            return bytes.ToArray();
        }
    }
}