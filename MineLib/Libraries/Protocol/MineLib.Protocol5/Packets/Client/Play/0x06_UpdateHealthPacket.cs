using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class UpdateHealthPacket : ClientPlayPacket
    {
		public Single Health;
		public Int16 Food;
		public Single FoodSaturation;

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
			Health = deserialiser.Read(Health);
			Food = deserialiser.Read(Food);
			FoodSaturation = deserialiser.Read(FoodSaturation);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Health);
            serializer.Write(Food);
            serializer.Write(FoodSaturation);
        }
    }
}