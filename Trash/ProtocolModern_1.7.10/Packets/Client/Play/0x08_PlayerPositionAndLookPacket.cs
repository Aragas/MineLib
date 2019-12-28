using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class PlayerPositionAndLookPacket : ProtobufPacket
    {
		public Double X;
		public Double Y;
		public Double Z;
		public Single Yaw;
		public Single Pitch;
		public Boolean OnGround;

        public override VarInt ID => ClientPlayPacketTypes.PlayerPositionAndLook;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);
			OnGround = reader.Read(OnGround);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Yaw);
			stream.Write(Pitch);
			stream.Write(OnGround);
          
            return this;
        }

    }
}