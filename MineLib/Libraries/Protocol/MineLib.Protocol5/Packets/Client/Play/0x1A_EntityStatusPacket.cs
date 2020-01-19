using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityStatusPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte EntityStatus;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			EntityStatus = deserialiser.Read(EntityStatus);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(EntityStatus);
        }
    }
}