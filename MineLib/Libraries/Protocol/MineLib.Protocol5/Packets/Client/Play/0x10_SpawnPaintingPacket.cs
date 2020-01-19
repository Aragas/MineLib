using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SpawnPaintingPacket : ClientPlayPacket
    {
		public VarInt EntityID;
		public String Title;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public Int32 Direction;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			Title = deserializer.Read(Title);
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Direction = deserializer.Read(Direction);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Title);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Direction);
        }
    }
}