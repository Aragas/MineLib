using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class UseBedPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public Int32 X;
		public Byte Y;
		public Int32 Z;

        public override VarInt ID => ClientPlayPacketTypes.UseBed;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
          
            return this;
        }

    }
}