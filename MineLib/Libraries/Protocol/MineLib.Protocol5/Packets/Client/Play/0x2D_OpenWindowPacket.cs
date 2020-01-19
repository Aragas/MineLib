using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class OpenWindowPacket : ClientPlayPacket
    {
		public Byte WindowID;
		public Byte InventoryType;
		public String WindowTitle;
		public Byte NumberOfSlots;
		public Boolean UseProvidedWindowTitle;
		public Int32 EntityID;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			WindowID = deserializer.Read(WindowID);
			InventoryType = deserializer.Read(InventoryType);
			WindowTitle = deserializer.Read(WindowTitle);
			NumberOfSlots = deserializer.Read(NumberOfSlots);
			UseProvidedWindowTitle = deserializer.Read(UseProvidedWindowTitle);
			EntityID = deserializer.Read(EntityID);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(InventoryType);
            serializer.Write(WindowTitle);
            serializer.Write(NumberOfSlots);
            serializer.Write(UseProvidedWindowTitle);
            serializer.Write(EntityID);
        }

    }
}