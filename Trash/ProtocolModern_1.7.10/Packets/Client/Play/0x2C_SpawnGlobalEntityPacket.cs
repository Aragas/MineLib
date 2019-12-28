using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class SpawnGlobalEntityPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public SByte Type;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;

        public override VarInt ID => ClientPlayPacketTypes.SpawnGlobalEntity;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Type = reader.Read(Type);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Type);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
          
            return this;
        }

    }
}