using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class TimeUpdatePacket : ClientPlayPacket
    {
		public Int64 AgeOfTheWorld;
		public Int64 TimeOfDay;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
			AgeOfTheWorld = deserializer.Read(AgeOfTheWorld);
			TimeOfDay = deserializer.Read(TimeOfDay);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(AgeOfTheWorld);
            serializer.Write(TimeOfDay);
        }
    }
}