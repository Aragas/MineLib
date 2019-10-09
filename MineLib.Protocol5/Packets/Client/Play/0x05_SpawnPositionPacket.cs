using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SpawnPositionPacket : ClientPlayPacket
    {
		public Int32 X;
		public Int32 Y;
		public Int32 Z;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
        }
    }
}