using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Login
{
    public class Disconnect2Packet : ProtobufPacket
    {
		public String JSONData;

        public override VarInt ID => ClientLoginPacketTypes.Disconnect2;

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