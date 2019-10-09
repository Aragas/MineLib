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

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Particlename = deserialiser.Read(Particlename);
			X = deserialiser.Read(X);
			Y = deserialiser.Read(Y);
			Z = deserialiser.Read(Z);
			OffsetX = deserialiser.Read(OffsetX);
			OffsetY = deserialiser.Read(OffsetY);
			OffsetZ = deserialiser.Read(OffsetZ);
			ParticleData = deserialiser.Read(ParticleData);
			NumberOfParticles = deserialiser.Read(NumberOfParticles);
        }

        public override void Serialize(ProtobufSerializer serializer)
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