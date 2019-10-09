using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class DisplayScoreboardPacket : ProtobufPacket
    {
		public SByte Position;
		public String ScoreName;

        public override VarInt ID => ClientPlayPacketTypes.DisplayScoreboard;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Position = reader.Read(Position);
			ScoreName = reader.Read(ScoreName);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Position);
			stream.Write(ScoreName);
          
            return this;
        }

    }
}