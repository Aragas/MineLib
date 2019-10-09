using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityRelativeMovePacket : ProtobufPacket
    {
		public Int32 EntityID;
		public SByte DX;
		public SByte DY;
		public SByte DZ;

        public override VarInt ID => ClientPlayPacketTypes.EntityRelativeMove;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			DX = reader.Read(DX);
			DY = reader.Read(DY);
			DZ = reader.Read(DZ);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(DX);
			stream.Write(DY);
			stream.Write(DZ);
          
            return this;
        }

    }
}