using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class BlockChangePacket : ProtobufPacket
    {
		public Int32 X;
		public Byte Y;
		public Int32 Z;
		public VarInt BlockID;
		public Byte BlockMetadata;

        public override VarInt ID => ClientPlayPacketTypes.BlockChange;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			BlockID = reader.Read(BlockID);
			BlockMetadata = reader.Read(BlockMetadata);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(BlockID);
			stream.Write(BlockMetadata);
          
            return this;
        }

    }
}