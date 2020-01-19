using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class UpdateSign2Packet : ServerPlayPacket
    {
		public Int32 X;
		public Int16 Y;
		public Int32 Z;
		public String Line1;
		public String Line2;
		public String Line3;
		public String Line4;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Line1 = deserializer.Read(Line1);
			Line2 = deserializer.Read(Line2);
			Line3 = deserializer.Read(Line3);
			Line4 = deserializer.Read(Line4);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Line1);
            serializer.Write(Line2);
            serializer.Write(Line3);
            serializer.Write(Line4);
        }
    }
}