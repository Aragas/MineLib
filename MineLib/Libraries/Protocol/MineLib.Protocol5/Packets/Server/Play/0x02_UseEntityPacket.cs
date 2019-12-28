using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class UseEntityPacket : ServerPlayPacket
    {
		public Int32 Target;
		public SByte Mouse;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Target = deserialiser.Read(Target);
			Mouse = deserialiser.Read(Mouse);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Target);
            serializer.Write(Mouse);
        }
    }
}