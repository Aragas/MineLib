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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			Type = deserializer.Read(Type);
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Pitch = deserializer.Read(Pitch);
			Yaw = deserializer.Read(Yaw);
			Data = deserializer.Read(Data);
        }

        public override void Serialize(IPacketSerializer serializer)
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