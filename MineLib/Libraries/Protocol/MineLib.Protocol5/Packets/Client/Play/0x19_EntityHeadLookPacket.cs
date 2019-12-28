using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityHeadLookPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte HeadYaw;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			HeadYaw = deserialiser.Read(HeadYaw);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(HeadYaw);
        }
    }
}