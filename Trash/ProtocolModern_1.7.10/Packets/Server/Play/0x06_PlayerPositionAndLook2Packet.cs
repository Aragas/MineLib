using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class PlayerPositionAndLook2Packet : ProtobufPacket
    {
		public Double X;
		public Double FeetY;
		public Double HeadY;
		public Double Z;
		public Single Yaw;
		public Single Pitch;
		public Boolean OnGround;

        public override VarInt ID => ServerPlayPacketTypes.PlayerPositionAndLook2;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			X = reader.Read(X);
			FeetY = reader.Read(FeetY);
			HeadY = reader.Read(HeadY);
			Z = reader.Read(Z);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);
			OnGround = reader.Read(OnGround);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(X);
			stream.Write(FeetY);
			stream.Write(HeadY);
			stream.Write(Z);
			stream.Write(Yaw);
			stream.Write(Pitch);
			stream.Write(OnGround);
          
            return this;
        }

    }
}