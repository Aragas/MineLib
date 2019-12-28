using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityStatusPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public SByte EntityStatus;

        public override VarInt ID => ClientPlayPacketTypes.EntityStatus;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			EntityStatus = reader.Read(EntityStatus);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(EntityStatus);
          
            return this;
        }

    }
}