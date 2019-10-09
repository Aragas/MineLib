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

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			WindowID = deserialiser.Read(WindowID);
			InventoryType = deserialiser.Read(InventoryType);
			WindowTitle = deserialiser.Read(WindowTitle);
			NumberOfSlots = deserialiser.Read(NumberOfSlots);
			UseProvidedWindowTitle = deserialiser.Read(UseProvidedWindowTitle);
			EntityID = deserialiser.Read(EntityID);
        }

        public override void Serialize(ProtobufSerializer serializer)
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