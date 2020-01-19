using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityPacket : ClientPlayPacket
    {
		public Int32 EntityID;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
        }
    }
}