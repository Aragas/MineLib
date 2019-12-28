using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Handshake
{
    public class HandshakePacket : ProtobufPacket
    {
		public VarInt ProtocolVersion { get; set; }
        public String ServerAddress { get; set; }
        public UInt16 ServerPort { get; set; }
        public VarInt NextState { get; set; }


        public override VarInt ID => ServerHandshakePacketTypes.Handshake;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			ProtocolVersion = reader.Read(ProtocolVersion);
			ServerAddress = reader.Read(ServerAddress);
			ServerPort = reader.Read(ServerPort);
			NextState = reader.Read(NextState);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(ProtocolVersion);
			stream.Write(ServerAddress);
			stream.Write(ServerPort);
			stream.Write(NextState);
          
            return this;
        }

    }
}