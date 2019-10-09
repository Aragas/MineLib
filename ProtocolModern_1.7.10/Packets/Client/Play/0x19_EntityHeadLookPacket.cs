using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityHeadLookPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public SByte HeadYaw;

        public override VarInt ID => ClientPlayPacketTypes.EntityHeadLook;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			HeadYaw = reader.Read(HeadYaw);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(HeadYaw);
          
            return this;
        }

    }
}