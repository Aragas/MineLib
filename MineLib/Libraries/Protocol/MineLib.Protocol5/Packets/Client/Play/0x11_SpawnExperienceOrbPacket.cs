using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SpawnExperienceOrbPacket : ClientPlayPacket
    {
		public VarInt EntityID;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public Int16 Count;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
        }
    }
}