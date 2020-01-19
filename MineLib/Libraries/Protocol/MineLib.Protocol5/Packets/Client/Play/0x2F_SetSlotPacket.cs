using System;
using Aragas.Network.IO;
using MineLib.Core;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SetSlotPacket : ClientPlayPacket
    {
		public SByte WindowID;
		public Int16 Slot;
		public ItemSlot SlotData;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			WindowID = deserializer.Read(WindowID);
			Slot = deserializer.Read(Slot);
			SlotData = deserializer.Read(SlotData);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(Slot);
            serializer.Write(SlotData);
        }
    }
}