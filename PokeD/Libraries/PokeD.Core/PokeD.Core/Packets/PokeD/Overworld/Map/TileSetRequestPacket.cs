using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Overworld.Map
{
    public class TileSetRequestPacket : PokeDPacket
    {
        public string[] TileSetNames { get; set; } = new string[0];


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            TileSetNames = deserializer.Read(TileSetNames);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(TileSetNames);
        }
    }
}
