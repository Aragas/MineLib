using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class EntityVelocityPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public Int16 VelocityX;
		public Int16 VelocityY;
		public Int16 VelocityZ;

        public override VarInt ID => ClientPlayPacketTypes.EntityVelocity;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			VelocityX = reader.Read(VelocityX);
			VelocityY = reader.Read(VelocityY);
			VelocityZ = reader.Read(VelocityZ);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(VelocityX);
			stream.Write(VelocityY);
			stream.Write(VelocityZ);
          
            return this;
        }

    }
}