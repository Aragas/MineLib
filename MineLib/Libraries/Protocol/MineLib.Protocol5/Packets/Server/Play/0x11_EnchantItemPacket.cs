using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class EnchantItemPacket : ServerPlayPacket
    {
		public SByte WindowID;
		public SByte Enchantment;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			WindowID = deserialiser.Read(WindowID);
			Enchantment = deserialiser.Read(Enchantment);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(Enchantment);
        }
    }
}