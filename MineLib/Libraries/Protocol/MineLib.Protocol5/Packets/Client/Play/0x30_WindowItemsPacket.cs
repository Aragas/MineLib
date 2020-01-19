using System;
using Aragas.Network.IO;
using MineLib.Core;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class WindowItemsPacket : ClientPlayPacket
    {
		public Byte WindowID;
		public Int16 Count;
		public ItemSlot[] SlotData;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			WindowID = deserialiser.Read(WindowID);
			SlotData = deserialiser.Read(SlotData);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(SlotData);
        }
    }
}