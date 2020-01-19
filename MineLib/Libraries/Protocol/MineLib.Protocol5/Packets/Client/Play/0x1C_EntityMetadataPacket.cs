using System;
using Aragas.Network.IO;
using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityMetadataPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public EntityMetadataList Metadata;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			Metadata = deserializer.Read(Metadata);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Metadata);
        }
    }
}