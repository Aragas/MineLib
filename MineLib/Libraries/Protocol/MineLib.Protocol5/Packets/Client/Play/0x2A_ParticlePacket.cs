using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ParticlePacket : ClientPlayPacket
    {
		public String Particlename;
		public Single X;
		public Single Y;
		public Single Z;
		public Single OffsetX;
		public Single OffsetY;
		public Single OffsetZ;
		public Single ParticleData;
		public Int32 NumberOfParticles;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Particlename = deserializer.Read(Particlename);
			X = deserializer.Read(X);
			Y = deserializer.Read(Y);
			Z = deserializer.Read(Z);
			OffsetX = deserializer.Read(OffsetX);
			OffsetY = deserializer.Read(OffsetY);
			OffsetZ = deserializer.Read(OffsetZ);
			ParticleData = deserializer.Read(ParticleData);
			NumberOfParticles = deserializer.Read(NumberOfParticles);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Particlename);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(OffsetX);
            serializer.Write(OffsetY);
            serializer.Write(OffsetZ);
            serializer.Write(ParticleData);
            serializer.Write(NumberOfParticles);
        }
    }
}