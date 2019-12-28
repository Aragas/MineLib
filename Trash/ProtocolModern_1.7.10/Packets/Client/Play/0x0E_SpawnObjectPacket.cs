using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class SpawnObjectPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public SByte Type;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public SByte Pitch;
		public SByte Yaw;
		public Int32 Data;

        public override VarInt ID => ClientPlayPacketTypes.SpawnObject;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Type = reader.Read(Type);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Pitch = reader.Read(Pitch);
			Yaw = reader.Read(Yaw);
			Data = reader.Read(Data);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Type);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Pitch);
			stream.Write(Yaw);
			stream.Write(Data);
          
            return this;
        }

    }
}