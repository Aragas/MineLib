using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class ChangeGameStatePacket : ProtobufPacket
    {
		public Byte Reason;
		public Single Value;

        public override VarInt ID => ClientPlayPacketTypes.ChangeGameState;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Reason = reader.Read(Reason);
			Value = reader.Read(Value);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Reason);
			stream.Write(Value);
          
            return this;
        }

    }
}