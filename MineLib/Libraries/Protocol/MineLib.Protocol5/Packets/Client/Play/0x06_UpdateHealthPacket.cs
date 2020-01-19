using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class UpdateHealthPacket : ClientPlayPacket
    {
		public Single Health;
		public Int16 Food;
		public Single FoodSaturation;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			Health = deserializer.Read(Health);
			Food = deserializer.Read(Food);
			FoodSaturation = deserializer.Read(FoodSaturation);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Health);
            serializer.Write(Food);
            serializer.Write(FoodSaturation);
        }
    }
}