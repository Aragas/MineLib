using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class PlayerAbilities2Packet : ServerPlayPacket
    {
		public SByte Flags;
		public Single FlyingSpeed;
		public Single WalkingSpeed;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Flags = deserialiser.Read(Flags);
			FlyingSpeed = deserialiser.Read(FlyingSpeed);
			WalkingSpeed = deserialiser.Read(WalkingSpeed);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Flags);
            serializer.Write(FlyingSpeed);
            serializer.Write(WalkingSpeed);
        }
    }
}