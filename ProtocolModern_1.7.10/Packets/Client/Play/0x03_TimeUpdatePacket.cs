using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class TimeUpdatePacket : ProtobufPacket
    {
		public Int64 AgeOfTheWorld;
		public Int64 TimeOfDay;

        public override VarInt ID => ClientPlayPacketTypes.TimeUpdate;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			AgeOfTheWorld = reader.Read(AgeOfTheWorld);
			TimeOfDay = reader.Read(TimeOfDay);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(AgeOfTheWorld);
			stream.Write(TimeOfDay);
          
            return this;
        }

    }
}