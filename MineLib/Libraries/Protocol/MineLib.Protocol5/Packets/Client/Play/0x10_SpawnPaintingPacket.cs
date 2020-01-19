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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			Title = deserialiser.Read(Title);
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Direction = deserialiser.Read(Direction);
        }

        public override void Serialize(IStreamSerializer serializer)
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