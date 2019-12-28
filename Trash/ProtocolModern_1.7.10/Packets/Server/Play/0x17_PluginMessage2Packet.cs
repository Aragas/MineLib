using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class PluginMessage2Packet : ProtobufPacket
    {
		public String Channel;
		public Byte[] Data;

        public override VarInt ID => ServerPlayPacketTypes.PluginMessage2;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Channel = reader.Read(Channel);
			var DataLength = reader.Read<Int16>();
			Data = reader.Read(Data, DataLength);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Channel);
			stream.Write((Int16) Data.Length);
			stream.Write(Data, false);
          
            return this;
        }

    }
}