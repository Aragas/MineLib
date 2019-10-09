using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class KeepAlive2Packet : ProtobufPacket
    {
		public Int32 KeepAliveID;

        public override VarInt ID => ServerPlayPacketTypes.KeepAlive2;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			KeepAliveID = reader.Read(KeepAliveID);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(KeepAliveID);
          
            return this;
        }

    }
}