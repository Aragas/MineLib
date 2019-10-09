using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class UpdateSignPacket : ProtobufPacket
    {
		public Int32 X;
		public Int16 Y;
		public Int32 Z;
		public String Line1;
		public String Line2;
		public String Line3;
		public String Line4;

        public override VarInt ID => ClientPlayPacketTypes.UpdateSign;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Line1 = reader.Read(Line1);
			Line2 = reader.Read(Line2);
			Line3 = reader.Read(Line3);
			Line4 = reader.Read(Line4);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Line1);
			stream.Write(Line2);
			stream.Write(Line3);
			stream.Write(Line4);
          
            return this;
        }

    }
}