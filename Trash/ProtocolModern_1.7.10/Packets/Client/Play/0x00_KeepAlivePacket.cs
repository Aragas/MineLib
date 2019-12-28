using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class KeepAlivePacket : ProtobufPacket
    {
		public Int32 KeepAliveID;

        public override VarInt ID => ClientPlayPacketTypes.KeepAlive;

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