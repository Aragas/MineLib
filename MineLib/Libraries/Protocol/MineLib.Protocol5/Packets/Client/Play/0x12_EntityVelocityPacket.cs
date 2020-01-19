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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			VelocityX = deserialiser.Read(VelocityX);
			VelocityY = deserialiser.Read(VelocityY);
			VelocityZ = deserialiser.Read(VelocityZ);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(VelocityX);
            serializer.Write(VelocityY);
            serializer.Write(VelocityZ);
        }
    }
}