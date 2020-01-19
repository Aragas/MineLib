using System;
using Aragas.Network.IO;
using MineLib.Core;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class CreativeInventoryActionPacket : ServerPlayPacket
    {
		public Int16 Slot;
		public ItemSlot ClickedItem;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Slot = deserializer.Read(Slot);
			ClickedItem = deserializer.Read(ClickedItem);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Slot);
            serializer.Write(ClickedItem);
        }
    }
}