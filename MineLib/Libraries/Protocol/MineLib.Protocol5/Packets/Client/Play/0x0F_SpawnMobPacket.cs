using System;
using Aragas.Network.Data;
using Aragas.Network.IO;
using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SpawnMobPacket : ClientPlayPacket
    {
		public VarInt EntityID;
		public Byte Type;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public SByte Yaw;
		public SByte Pitch;
		public SByte HeadPitch;
		public Int16 VelocityX;
		public Int16 VelocityY;
		public Int16 VelocityZ;
		public EntityMetadataList Metadata;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			EntityID = deserializer.Read(EntityID);
			Type = deserializer.Read(Type);
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			Yaw = deserializer.Read(Yaw);
			Pitch = deserializer.Read(Pitch);
			HeadPitch = deserializer.Read(HeadPitch);
			VelocityX = deserializer.Read(VelocityX);
			VelocityY = deserializer.Read(VelocityY);
			VelocityZ = deserializer.Read(VelocityZ);
            Metadata = deserializer.Read(Metadata);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(Type);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
            serializer.Write(HeadPitch);
            serializer.Write(VelocityX);
            serializer.Write(VelocityY);
            serializer.Write(VelocityZ);
            serializer.Write(Metadata);
        }
    }
}