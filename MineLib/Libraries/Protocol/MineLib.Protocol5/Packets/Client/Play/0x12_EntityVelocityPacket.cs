using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityVelocityPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Int16 VelocityX;
		public Int16 VelocityY;
		public Int16 VelocityZ;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			VelocityX = deserializer.Read(VelocityX);
			VelocityY = deserializer.Read(VelocityY);
			VelocityZ = deserializer.Read(VelocityZ);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(VelocityX);
            serializer.Write(VelocityY);
            serializer.Write(VelocityZ);
        }
    }
}