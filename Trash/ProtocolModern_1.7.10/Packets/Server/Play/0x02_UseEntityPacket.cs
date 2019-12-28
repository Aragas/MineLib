using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class UseEntityPacket : ProtobufPacket
    {
		public Int32 Target;
		public SByte Mouse;

        public override VarInt ID => ServerPlayPacketTypes.UseEntity;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Target = reader.Read(Target);
			Mouse = reader.Read(Mouse);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Target);
			stream.Write(Mouse);
          
            return this;
        }

    }
}