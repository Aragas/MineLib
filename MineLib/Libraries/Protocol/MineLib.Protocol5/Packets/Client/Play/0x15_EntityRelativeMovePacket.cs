using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class EntityRelativeMovePacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public SByte DX;
		public SByte DY;
		public SByte DZ;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			DX = deserialiser.Read(DX);
			DY = deserialiser.Read(DY);
			DZ = deserialiser.Read(DZ);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(DX);
            serializer.Write(DY);
            serializer.Write(DZ);
        }
    }
}