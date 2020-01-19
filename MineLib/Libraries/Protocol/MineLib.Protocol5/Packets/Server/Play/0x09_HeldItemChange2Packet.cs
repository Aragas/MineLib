using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class HeldItemChange2Packet : ServerPlayPacket
    {
		public Int16 Slot;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Slot = deserializer.Read(Slot);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Slot);
        }
    }
}