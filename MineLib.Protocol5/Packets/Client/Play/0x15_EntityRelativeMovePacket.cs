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

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			DX = deserialiser.Read(DX);
			DY = deserialiser.Read(DY);
			DZ = deserialiser.Read(DZ);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(DX);
            serializer.Write(DY);
            serializer.Write(DZ);
        }
    }
}