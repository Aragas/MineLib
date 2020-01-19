using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class HeldItemChange2Packet : ServerPlayPacket
    {
		public Int16 Slot;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Slot = deserialiser.Read(Slot);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Slot);
        }
    }
}