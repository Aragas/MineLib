using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class HeldItemChangePacket : ClientPlayPacket
    {
		public SByte Slot;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			Slot = deserialiser.Read(Slot);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Slot);
        }
    }
}