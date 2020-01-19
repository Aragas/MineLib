using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityHeadLookPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte HeadYaw;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			HeadYaw = deserialiser.Read(HeadYaw);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(HeadYaw);
        }
    }
}