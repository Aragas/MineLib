using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class PlayerLookPacket : ProtobufPacket
    {
		public Single Yaw;
		public Single Pitch;
		public Boolean OnGround;

        public override VarInt ID => ServerPlayPacketTypes.PlayerLook;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);
			OnGround = reader.Read(OnGround);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Yaw);
			stream.Write(Pitch);
			stream.Write(OnGround);
          
            return this;
        }

    }
}