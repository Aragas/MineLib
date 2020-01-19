using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class UseBedPacket : ClientPlayPacket
    {
		public Int32 EntityID;
		public Int32 X;
		public Byte Y;
		public Int32 Z;

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