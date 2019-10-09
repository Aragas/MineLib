using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using MineLib.Core.Data;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class WindowItemsPacket : ProtobufPacket
    {
		public Byte WindowID;
		public Int16 Count;
		public ItemStack[] SlotData;

        public override VarInt ID => ClientPlayPacketTypes.WindowItems;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			SlotData = reader.Read(SlotData);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(WindowID);
			stream.Write(SlotData);
          
            return this;
        }

    }
}