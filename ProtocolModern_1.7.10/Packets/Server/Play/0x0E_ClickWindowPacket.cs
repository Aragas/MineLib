using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using MineLib.Core.Data;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class ClickWindowPacket : ProtobufPacket
    {
		public SByte WindowID;
		public Int16 Slot;
		public SByte Button;
		public Int16 ActionNumber;
		public SByte Mode;
		public ItemStack ClickedItem;

        public override VarInt ID => ServerPlayPacketTypes.ClickWindow;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			Slot = reader.Read(Slot);
			Button = reader.Read(Button);
			ActionNumber = reader.Read(ActionNumber);
			Mode = reader.Read(Mode);
			ClickedItem = reader.Read(ClickedItem);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(WindowID);
			stream.Write(Slot);
			stream.Write(Button);
			stream.Write(ActionNumber);
			stream.Write(Mode);
			stream.Write(ClickedItem);
          
            return this;
        }

    }
}