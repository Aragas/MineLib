using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class SpawnExperienceOrbPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public Int16 Count;

        public override VarInt ID => ClientPlayPacketTypes.SpawnExperienceOrb;

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