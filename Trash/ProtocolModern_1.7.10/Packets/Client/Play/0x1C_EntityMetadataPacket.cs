using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Data;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityMetadataPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public EntityMetadataList Metadata;

        public override VarInt ID => ClientPlayPacketTypes.EntityMetadata;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Metadata = reader.Read(Metadata);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Metadata);
          
            return this;
        }

    }
}