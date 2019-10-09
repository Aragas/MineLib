using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class Animation2Packet : ProtobufPacket
    {
		public Int32 EntityID;
		public SByte Animation;

        public override VarInt ID => ServerPlayPacketTypes.Animation2;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Animation = reader.Read(Animation);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Animation);
          
            return this;
        }

    }
}