using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityLookPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte Yaw;
		public SByte Pitch;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			Yaw = deserializer.Read(Yaw);
			Pitch = deserializer.Read(Pitch);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
        }
    }
}