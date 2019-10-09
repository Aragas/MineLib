using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityLookPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public SByte Yaw;
		public SByte Pitch;

        public override VarInt ID => ClientPlayPacketTypes.EntityLook;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Yaw);
			stream.Write(Pitch);
          
            return this;
        }

    }
}