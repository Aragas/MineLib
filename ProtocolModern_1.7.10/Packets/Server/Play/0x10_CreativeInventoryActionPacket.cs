using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using MineLib.Core.Data;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class CreativeInventoryActionPacket : ProtobufPacket
    {
		public Int16 Slot;
		public ItemStack ClickedItem;

        public override VarInt ID => ServerPlayPacketTypes.CreativeInventoryAction;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Slot = reader.Read(Slot);
			ClickedItem = reader.Read(ClickedItem);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Slot);
			stream.Write(ClickedItem);
          
            return this;
        }

    }
}