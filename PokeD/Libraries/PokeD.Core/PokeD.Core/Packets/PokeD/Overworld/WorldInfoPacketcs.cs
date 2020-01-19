using System;

using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Overworld
{
    public class WorldInfoPacket : PokeDPacket
    {
        public TimeSpan Time { get; set; }
        public byte Season { get; set; }
        public byte Weather { get; set; }
        public byte Event { get; set; }


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Time = deserializer.Read(Time);
            Season = deserializer.Read(Season);
            Weather = deserializer.Read(Weather);
            Event = deserializer.Read(Event);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Time);
            serializer.Write(Season);
            serializer.Write(Weather);
            serializer.Write(Event);
        }
    }
}
