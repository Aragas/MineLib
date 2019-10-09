using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class UpdateBlockEntityPacket : ProtobufPacket
    {
		public Int32 X;
		public Int16 Y;
		public Int32 Z;
		public Byte Action;
		public Byte[] NBTData;

        public override VarInt ID => ClientPlayPacketTypes.UpdateBlockEntity;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Action = reader.Read(Action);
            var NBTDataLength = reader.Read<Int16>();
            NBTData = reader.Read(NBTData, NBTDataLength);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Action);
            stream.Write((Int16) NBTData.Length);
            stream.Write(NBTData);
          
            return this;
        }

    }
}