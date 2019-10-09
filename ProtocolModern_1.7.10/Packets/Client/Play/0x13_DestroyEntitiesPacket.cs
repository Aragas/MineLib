using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class DestroyEntitiesPacket : ProtobufPacket
    {
		public Int32[] EntityIDs;

        public override VarInt ID => ClientPlayPacketTypes.DestroyEntities;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
            var EntityIDsLength = reader.Read<Byte>();
            EntityIDs = reader.Read(EntityIDs, EntityIDsLength);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
            stream.Write((Byte) EntityIDs.Length);
            stream.Write(EntityIDs, false);
          
            return this;
        }

    }
}