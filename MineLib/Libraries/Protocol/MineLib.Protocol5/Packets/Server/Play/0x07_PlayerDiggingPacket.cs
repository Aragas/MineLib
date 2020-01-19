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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Status = deserialiser.Read(Status);
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Face = deserialiser.Read(Face);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Status);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Face);
        }
    }
}