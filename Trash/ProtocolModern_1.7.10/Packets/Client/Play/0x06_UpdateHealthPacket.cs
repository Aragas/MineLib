using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class UpdateHealthPacket : ProtobufPacket
    {
		public Single Health;
		public Int16 Food;
		public Single FoodSaturation;

        public override VarInt ID => ClientPlayPacketTypes.UpdateHealth;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Health = reader.Read(Health);
			Food = reader.Read(Food);
			FoodSaturation = reader.Read(FoodSaturation);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Health);
			stream.Write(Food);
			stream.Write(FoodSaturation);
          
            return this;
        }

    }
}