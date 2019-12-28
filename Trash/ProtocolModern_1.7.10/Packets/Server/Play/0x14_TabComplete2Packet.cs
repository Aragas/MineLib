using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class TabComplete2Packet : ProtobufPacket
    {
		public String Text;

        public override VarInt ID => ServerPlayPacketTypes.TabComplete2;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Text = reader.Read(Text);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Text);
          
            return this;
        }

    }
}