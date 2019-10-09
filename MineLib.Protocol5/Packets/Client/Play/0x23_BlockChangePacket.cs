using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class BlockChangePacket : ClientPlayPacket
    {
		public Int32 X;
		public Byte Y;
		public Int32 Z;
		public VarInt BlockID;
		public Byte BlockMetadata;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			BlockID = deserialiser.Read(BlockID);
			BlockMetadata = deserialiser.Read(BlockMetadata);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(BlockID);
            serializer.Write(BlockMetadata);
        }
    }
}