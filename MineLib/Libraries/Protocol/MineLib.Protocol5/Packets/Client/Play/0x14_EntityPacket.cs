using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityPacket : ClientPlayPacket
    {
		public Int32 EntityID;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
        }
    }
}