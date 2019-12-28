using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class PlayerDiggingPacket : ProtobufPacket
    {
		public SByte Status;
		public Int32 X;
		public Byte Y;
		public Int32 Z;
		public SByte Face;

        public override VarInt ID => ServerPlayPacketTypes.PlayerDigging;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Status = reader.Read(Status);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Face = reader.Read(Face);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Status);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Face);
          
            return this;
        }

    }
}