using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using MineLib.Core.Data;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class PlayerBlockPlacementPacket : ProtobufPacket
    {
		public Int32 X;
		public Byte Y;
		public Int32 Z;
		public SByte Direction;
		public ItemStack HeldItem;
		public SByte CursorPositionX;
		public SByte CursorPositionY;
		public SByte CursorPositionZ;

        public override VarInt ID => ServerPlayPacketTypes.PlayerBlockPlacement;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Direction = reader.Read(Direction);
			HeldItem = reader.Read(HeldItem);
			CursorPositionX = reader.Read(CursorPositionX);
			CursorPositionY = reader.Read(CursorPositionY);
			CursorPositionZ = reader.Read(CursorPositionZ);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Direction);
			stream.Write(HeldItem);
			stream.Write(CursorPositionX);
			stream.Write(CursorPositionY);
			stream.Write(CursorPositionZ);
          
            return this;
        }

    }
}