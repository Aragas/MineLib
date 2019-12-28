using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class ConfirmTransaction2Packet : ProtobufPacket
    {
		public SByte WindowID;
		public Int16 ActionNumber;
		public Boolean Accepted;

        public override VarInt ID => ServerPlayPacketTypes.ConfirmTransaction2;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			ActionNumber = reader.Read(ActionNumber);
			Accepted = reader.Read(Accepted);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(WindowID);
			stream.Write(ActionNumber);
			stream.Write(Accepted);
          
            return this;
        }

    }
}