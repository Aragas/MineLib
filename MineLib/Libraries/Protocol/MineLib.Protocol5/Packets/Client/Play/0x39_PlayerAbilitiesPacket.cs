using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class PlayerAbilitiesPacket : ClientPlayPacket
    {
		public SByte Flags;
		public Single FlyingSpeed;
		public Single WalkingSpeed;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Flags = deserializer.Read(Flags);
			FlyingSpeed = deserializer.Read(FlyingSpeed);
			WalkingSpeed = deserializer.Read(WalkingSpeed);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Flags);
            serializer.Write(FlyingSpeed);
            serializer.Write(WalkingSpeed);
        }

    }
}