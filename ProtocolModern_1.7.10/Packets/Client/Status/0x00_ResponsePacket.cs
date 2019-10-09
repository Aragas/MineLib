using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Status
{
    public class ResponsePacket : ProtobufPacket
    {
		public String JSONResponse;

        public override VarInt ID => ClientStatusPacketTypes.Response;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			JSONResponse = reader.Read(JSONResponse);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(JSONResponse);
          
            return this;
        }

    }
}