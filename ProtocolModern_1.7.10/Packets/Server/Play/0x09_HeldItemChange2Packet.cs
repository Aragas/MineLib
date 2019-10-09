using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class HeldItemChange2Packet : ProtobufPacket
    {
		public Int16 Slot;

        public override VarInt ID => ServerPlayPacketTypes.HeldItemChange2;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Slot = reader.Read(Slot);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Slot);
          
            return this;
        }

    }
}