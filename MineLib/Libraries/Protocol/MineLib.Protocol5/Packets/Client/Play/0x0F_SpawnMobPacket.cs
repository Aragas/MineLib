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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			EntityID = deserialiser.Read(EntityID);
			Type = deserialiser.Read(Type);
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			Yaw = deserialiser.Read(Yaw);
			Pitch = deserialiser.Read(Pitch);
			HeadPitch = deserialiser.Read(HeadPitch);
			VelocityX = deserialiser.Read(VelocityX);
			VelocityY = deserialiser.Read(VelocityY);
			VelocityZ = deserialiser.Read(VelocityZ);
            Metadata = deserialiser.Read(Metadata);
        }

        public override void Serialize(IStreamSerializer serializer)
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