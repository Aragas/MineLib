using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using MineLib.Core.Data;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class SetSlotPacket : ProtobufPacket
    {
		public SByte WindowID;
		public Int16 Slot;
		public ItemStack SlotData;

        public override VarInt ID => ClientPlayPacketTypes.SetSlot;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			Slot = reader.Read(Slot);
			SlotData = reader.Read(SlotData);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(WindowID);
			stream.Write(Slot);
			stream.Write(SlotData);
          
            return this;
        }

    }
}