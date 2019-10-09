using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class ChatMessage2Packet : ProtobufPacket
    {
		public String Message;

        public override VarInt ID => ServerPlayPacketTypes.ChatMessage2;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Message = reader.Read(Message);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Message);
          
            return this;
        }

    }
}