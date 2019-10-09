using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SpawnObjectPacket : ClientPlayPacket
    {
		public VarInt EntityID;
		public SByte Type;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public SByte Pitch;
		public SByte Yaw;
		public Int32 Data;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			Type = deserialiser.Read(Type);
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Pitch = deserialiser.Read(Pitch);
			Yaw = deserialiser.Read(Yaw);
			Data = deserialiser.Read(Data);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Type);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Pitch);
            serializer.Write(Yaw);
            serializer.Write(Data);
        }
    }
}