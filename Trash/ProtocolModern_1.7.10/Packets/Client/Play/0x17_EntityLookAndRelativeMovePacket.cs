using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityLookAndRelativeMovePacket : ProtobufPacket
    {
		public Int32 EntityID;
		public SByte DX;
		public SByte DY;
		public SByte DZ;
		public SByte Yaw;
		public SByte Pitch;

        public override VarInt ID => ClientPlayPacketTypes.EntityLookAndRelativeMove;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			DX = reader.Read(DX);
			DY = reader.Read(DY);
			DZ = reader.Read(DZ);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(DX);
			stream.Write(DY);
			stream.Write(DZ);
			stream.Write(Yaw);
			stream.Write(Pitch);
          
            return this;
        }

    }
}