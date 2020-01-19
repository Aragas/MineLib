using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityHeadLookPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte HeadYaw;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			HeadYaw = deserializer.Read(HeadYaw);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(HeadYaw);
        }
    }
}