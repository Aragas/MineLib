using System;
using System.Text;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class PluginMessagePacket : ProtobufPacket
    {
		public String Channel;
		public Byte[] Data;
        public string DataString => Encoding.UTF8.GetString(Data, 0, Data.Length);

        public override VarInt ID => ClientPlayPacketTypes.PluginMessage;

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