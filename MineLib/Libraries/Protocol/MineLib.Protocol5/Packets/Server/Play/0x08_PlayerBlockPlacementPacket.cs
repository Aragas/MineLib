using System;

using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PlayerBlockPlacementPacket : ServerPlayPacket
    {
		public Int32 X;
		public Byte Y;
		public Int32 Z;
		public SByte Direction;
		public ItemSlot HeldItem;
		public SByte CursorPositionX;
		public SByte CursorPositionY;
		public SByte CursorPositionZ;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Direction = deserializer.Read(Direction);
			HeldItem = deserializer.Read(HeldItem);
			CursorPositionX = deserializer.Read(CursorPositionX);
			CursorPositionY = deserializer.Read(CursorPositionY);
			CursorPositionZ = deserializer.Read(CursorPositionZ);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Direction);
            serializer.Write(HeldItem);
            serializer.Write(CursorPositionX);
            serializer.Write(CursorPositionY);
            serializer.Write(CursorPositionZ);
        }
    }
}