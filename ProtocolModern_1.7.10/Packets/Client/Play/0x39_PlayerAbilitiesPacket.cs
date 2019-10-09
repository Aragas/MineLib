using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class PlayerAbilitiesPacket : ProtobufPacket
    {
		public SByte Flags;
		public Single FlyingSpeed;
		public Single WalkingSpeed;

        public override VarInt ID => ClientPlayPacketTypes.PlayerAbilities;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Flags = reader.Read(Flags);
			FlyingSpeed = reader.Read(FlyingSpeed);
			WalkingSpeed = reader.Read(WalkingSpeed);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Flags);
			stream.Write(FlyingSpeed);
			stream.Write(WalkingSpeed);
          
            return this;
        }

    }
}