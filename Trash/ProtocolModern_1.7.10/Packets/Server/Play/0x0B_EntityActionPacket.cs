using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Play
{
    public class EntityActionPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public SByte ActionID;
		public Int32 JumpBoost;

        public override VarInt ID => ServerPlayPacketTypes.EntityAction;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			ActionID = reader.Read(ActionID);
			JumpBoost = reader.Read(JumpBoost);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(ActionID);
			stream.Write(JumpBoost);
          
            return this;
        }

    }
}