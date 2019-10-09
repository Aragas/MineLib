using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Handshake
{
    public class HandshakePacket : ServerHandshakePacket
    {
		public VarInt ProtocolVersion { get; set; }
        public String ServerAddress { get; set; }
        public UInt16 ServerPort { get; set; }
        public VarInt NextState { get; set; }

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			ProtocolVersion = deserialiser.Read(ProtocolVersion);
			ServerAddress = deserialiser.Read(ServerAddress);
			ServerPort = deserialiser.Read(ServerPort);
			NextState = deserialiser.Read(NextState);
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