using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class AnimationPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public Byte Animation;

        public override VarInt ID => ClientPlayPacketTypes.Animation;

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