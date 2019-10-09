using System;
using Aragas.Network.IO;
using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityMetadataPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public EntityMetadataList Metadata;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			Metadata = deserialiser.Read(Metadata);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Metadata);
        }
    }
}