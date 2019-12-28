using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class TabCompletePacket : ProtobufPacket
    {
		public String[] Match;

        public override VarInt ID => ClientPlayPacketTypes.TabComplete;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Match = reader.Read(Match);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Match);
          
            return this;
        }

    }
}