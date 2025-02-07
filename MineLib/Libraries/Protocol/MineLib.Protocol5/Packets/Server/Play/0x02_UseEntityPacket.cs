using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class UseEntityPacket : ServerPlayPacket
    {
		public Int32 Target;
		public SByte Mouse;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Target = deserializer.Read(Target);
			Mouse = deserializer.Read(Mouse);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Target);
            serializer.Write(Mouse);
        }
    }
}