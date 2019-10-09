using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class SpawnPaintingPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public String Title;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public Int32 Direction;

        public override VarInt ID => ClientPlayPacketTypes.SpawnPainting;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Title = reader.Read(Title);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Direction = reader.Read(Direction);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Title);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Direction);
          
            return this;
        }

    }
}