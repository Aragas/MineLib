using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.IO;

using System;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets.Serverbound
{
    [Packet(0x00)]
    public sealed class HandshakePacket : HandshakeStatePacket
    {
		public VarInt ProtocolVersion { get; set; }
        public String ServerAddress { get; set; } = default!;
        public UInt16 ServerPort { get; set; }
        public VarInt NextState { get; set; }

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
			ProtocolVersion = deserializer.Read(ProtocolVersion);
			ServerAddress = deserializer.Read(ServerAddress);
			ServerPort = deserializer.Read(ServerPort);
			NextState = deserializer.Read(NextState);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(ProtocolVersion);
            serializer.Write(ServerAddress);
            serializer.Write(ServerPort);
            serializer.Write(NextState);
        }
    }
}