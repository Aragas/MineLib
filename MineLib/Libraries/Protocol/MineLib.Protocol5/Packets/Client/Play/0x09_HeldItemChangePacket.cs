using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class HeldItemChangePacket : ClientPlayPacket
    {
		public SByte Slot;

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