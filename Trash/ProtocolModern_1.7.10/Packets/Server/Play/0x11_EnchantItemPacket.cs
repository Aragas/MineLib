using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class EnchantItemPacket : ProtobufPacket
    {
		public SByte WindowID;
		public SByte Enchantment;

        public override VarInt ID => ServerPlayPacketTypes.EnchantItem;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			Enchantment = reader.Read(Enchantment);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(WindowID);
			stream.Write(Enchantment);
          
            return this;
        }

    }
}