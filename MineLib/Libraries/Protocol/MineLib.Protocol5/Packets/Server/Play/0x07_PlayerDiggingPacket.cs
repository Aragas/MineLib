using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PlayerDiggingPacket : ServerPlayPacket
    {
		public SByte Status;
		public Int32 X;
		public Byte Y;
		public Int32 Z;
		public SByte Face;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Status = deserializer.Read(Status);
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Face = deserializer.Read(Face);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Status);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Face);
        }
    }
}