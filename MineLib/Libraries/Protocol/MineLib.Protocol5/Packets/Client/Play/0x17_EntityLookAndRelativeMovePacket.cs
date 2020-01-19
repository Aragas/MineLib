using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityLookAndRelativeMovePacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte DX;
		public SByte DY;
		public SByte DZ;
		public SByte Yaw;
		public SByte Pitch;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			DX = deserializer.Read(DX);
			DY = deserializer.Read(DY);
			DZ = deserializer.Read(DZ);
			Yaw = deserializer.Read(Yaw);
			Pitch = deserializer.Read(Pitch);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(DX);
            serializer.Write(DY);
            serializer.Write(DZ);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
        }
    }
}