using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class HeldItemChangePacket : ClientPlayPacket
    {
		public SByte Slot;

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