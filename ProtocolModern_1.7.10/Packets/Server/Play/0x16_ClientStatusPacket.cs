using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class ClientStatusPacket : ProtobufPacket
    {
		public SByte ActionID;

        public override VarInt ID => ServerPlayPacketTypes.ClientStatus;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			ActionID = reader.Read(ActionID);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(ActionID);
          
            return this;
        }

    }
}