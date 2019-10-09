using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class HeldItemChangePacket : ProtobufPacket
    {
		public SByte Slot;

        public override VarInt ID => ClientPlayPacketTypes.HeldItemChange;

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