using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class TimeUpdatePacket : ClientPlayPacket
    {
		public Int64 AgeOfTheWorld;
		public Int64 TimeOfDay;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
			AgeOfTheWorld = deserialiser.Read(AgeOfTheWorld);
			TimeOfDay = deserialiser.Read(TimeOfDay);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(AgeOfTheWorld);
            serializer.Write(TimeOfDay);
        }
    }
}