using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class HeldItemChange2Packet : ServerPlayPacket
    {
		public Int16 Slot;

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