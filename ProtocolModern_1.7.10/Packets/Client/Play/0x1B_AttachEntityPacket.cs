using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class AttachEntityPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public Int32 VehicleID;
		public Boolean Leash;

        public override VarInt ID => ClientPlayPacketTypes.AttachEntity;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			VehicleID = reader.Read(VehicleID);
			Leash = reader.Read(Leash);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(VehicleID);
			stream.Write(Leash);
          
            return this;
        }

    }
}