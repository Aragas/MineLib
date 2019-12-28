using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class WindowPropertyPacket : ProtobufPacket
    {
		public Byte WindowID;
		public Int16 Property;
		public Int16 Value;

        public override VarInt ID => ClientPlayPacketTypes.WindowProperty;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			Property = reader.Read(Property);
			Value = reader.Read(Value);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(WindowID);
			stream.Write(Property);
			stream.Write(Value);
          
            return this;
        }

    }
}