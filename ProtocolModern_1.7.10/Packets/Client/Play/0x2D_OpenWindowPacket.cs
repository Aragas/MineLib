using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class OpenWindowPacket : ProtobufPacket
    {
		public Byte WindowID;
		public Byte InventoryType;
		public String WindowTitle;
		public Byte NumberOfSlots;
		public Boolean UseProvidedWindowTitle;
		public Int32 EntityID;

        public override VarInt ID => ClientPlayPacketTypes.OpenWindow;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			InventoryType = reader.Read(InventoryType);
			WindowTitle = reader.Read(WindowTitle);
			NumberOfSlots = reader.Read(NumberOfSlots);
			UseProvidedWindowTitle = reader.Read(UseProvidedWindowTitle);
			EntityID = reader.Read(EntityID);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(WindowID);
			stream.Write(InventoryType);
			stream.Write(WindowTitle);
			stream.Write(NumberOfSlots);
			stream.Write(UseProvidedWindowTitle);
			stream.Write(EntityID);
          
            return this;
        }

    }
}