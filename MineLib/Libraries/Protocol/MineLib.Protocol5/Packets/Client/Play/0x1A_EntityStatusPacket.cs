using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityStatusPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte EntityStatus;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			EntityStatus = deserializer.Read(EntityStatus);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(EntityStatus);
        }
    }
}