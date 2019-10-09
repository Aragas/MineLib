using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class MapsPacket : ProtobufPacket
    {
		public VarInt ItemDamage;
		public Byte[] Data;

        public override VarInt ID => ClientPlayPacketTypes.Maps;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			ItemDamage = reader.Read(ItemDamage);
			var DataLength = reader.Read<Int16>();
			Data = reader.Read(Data, DataLength);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(ItemDamage);
			stream.Write((Int16) Data.Length);
			stream.Write(Data, false);
          
            return this;
        }

    }
}