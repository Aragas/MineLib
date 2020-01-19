using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Play
{
    public class EnchantItemPacket : ServerPlayPacket
    {
		public SByte WindowID;
		public SByte Enchantment;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			WindowID = deserializer.Read(WindowID);
			Enchantment = deserializer.Read(Enchantment);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(WindowID);
            serializer.Write(Enchantment);
        }
    }
}