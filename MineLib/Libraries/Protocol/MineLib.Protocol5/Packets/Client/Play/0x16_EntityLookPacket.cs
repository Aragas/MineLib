using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityLookPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte Yaw;
		public SByte Pitch;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			Yaw = deserialiser.Read(Yaw);
			Pitch = deserialiser.Read(Pitch);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
        }
    }
}