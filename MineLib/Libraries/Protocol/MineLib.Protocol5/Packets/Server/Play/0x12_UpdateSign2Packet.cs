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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Line1 = deserialiser.Read(Line1);
			Line2 = deserialiser.Read(Line2);
			Line3 = deserialiser.Read(Line3);
			Line4 = deserialiser.Read(Line4);
        }

        public override void Serialize(IStreamSerializer serializer)
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