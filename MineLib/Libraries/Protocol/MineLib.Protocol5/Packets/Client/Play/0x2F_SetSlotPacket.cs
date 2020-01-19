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

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			WindowID = deserialiser.Read(WindowID);
			Slot = deserialiser.Read(Slot);
			SlotData = deserialiser.Read(SlotData);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(Slot);
            serializer.Write(SlotData);
        }
    }
}