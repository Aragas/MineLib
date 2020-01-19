using System;

using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class ClickWindowPacket : ServerPlayPacket
    {
		public SByte WindowID;
		public Int16 Slot;
		public SByte Button;
		public Int16 ActionNumber;
		public SByte Mode;
		public ItemSlot ClickedItem;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			WindowID = deserializer.Read(WindowID);
			Slot = deserializer.Read(Slot);
			Button = deserializer.Read(Button);
			ActionNumber = deserializer.Read(ActionNumber);
			Mode = deserializer.Read(Mode);
			ClickedItem = deserializer.Read(ClickedItem);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(Slot);
            serializer.Write(Button);
            serializer.Write(ActionNumber);
            serializer.Write(Mode);
            serializer.Write(ClickedItem);
        }
    }
}