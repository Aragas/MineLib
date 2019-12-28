using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class DisconnectPacket : ProtobufPacket
    {
		public String Reason;

        public override VarInt ID => ClientPlayPacketTypes.Disconnect;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Reason = reader.Read(Reason);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Reason);
          
            return this;
        }

    }
}