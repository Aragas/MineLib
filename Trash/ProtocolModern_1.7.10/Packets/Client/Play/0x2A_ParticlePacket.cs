using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class ParticlePacket : ProtobufPacket
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

        public override VarInt ID => ClientPlayPacketTypes.Particle;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
			Particlename = reader.Read(Particlename);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			OffsetX = reader.Read(OffsetX);
			OffsetY = reader.Read(OffsetY);
			OffsetZ = reader.Read(OffsetZ);
			ParticleData = reader.Read(ParticleData);
			NumberOfParticles = reader.Read(NumberOfParticles);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
			stream.Write(Particlename);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(OffsetX);
			stream.Write(OffsetY);
			stream.Write(OffsetZ);
			stream.Write(ParticleData);
			stream.Write(NumberOfParticles);
          
            return this;
        }

    }
}