using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class ChatMessagePacket : ProtobufPacket
    {
		public String JSONData;

        public override VarInt ID => ClientPlayPacketTypes.ChatMessage;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			JSONData = reader.Read(JSONData);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(JSONData);
          
            return this;
        }

    }
}