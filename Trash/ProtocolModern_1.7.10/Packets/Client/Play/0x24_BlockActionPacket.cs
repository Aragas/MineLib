using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class BlockActionPacket : ProtobufPacket
    {
		public Int32 X;
		public Int16 Y;
		public Int32 Z;
		public Byte Byte1;
		public Byte Byte2;
		public VarInt BlockType;

        public override VarInt ID => ClientPlayPacketTypes.BlockAction;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Byte1 = reader.Read(Byte1);
			Byte2 = reader.Read(Byte2);
			BlockType = reader.Read(BlockType);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Byte1);
			stream.Write(Byte2);
			stream.Write(BlockType);
          
            return this;
        }

    }
}