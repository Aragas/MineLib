using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class PlayerPositionPacket : ProtobufPacket
    {
		public Double X;
		public Double FeetY;
		public Double HeadY;
		public Double Z;
		public Boolean OnGround;

        public override VarInt ID => ServerPlayPacketTypes.PlayerPosition;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			X = reader.Read(X);
			FeetY = reader.Read(FeetY);
			HeadY = reader.Read(HeadY);
			Z = reader.Read(Z);
			OnGround = reader.Read(OnGround);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(X);
			stream.Write(FeetY);
			stream.Write(HeadY);
			stream.Write(Z);
			stream.Write(OnGround);
          
            return this;
        }

    }
}