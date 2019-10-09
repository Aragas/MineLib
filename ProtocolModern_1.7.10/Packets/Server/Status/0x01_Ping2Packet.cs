using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Status
{
    public class Ping2Packet : ProtobufPacket
    {
		public Int64 Time;

        public override VarInt ID => ServerStatusPacketTypes.Ping2;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Time = reader.Read(Time);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Time);
          
            return this;
        }

    }
}