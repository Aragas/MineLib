using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class PlayerPacket : ProtobufPacket
    {
		public Boolean OnGround;

        public override VarInt ID => ServerPlayPacketTypes.Player;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			OnGround = reader.Read(OnGround);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(OnGround);
          
            return this;
        }

    }
}