using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityTeleportPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public SByte Yaw;
		public SByte Pitch;

        public override VarInt ID => ClientPlayPacketTypes.EntityTeleport;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Yaw);
			stream.Write(Pitch);
          
            return this;
        }

    }
}