using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Overworld.Map
{
    public class FileHash
    {
        public string Name { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
    }

    public class MapPacket : PokeDPacket
    {
        public string MapData { get; set; } = string.Empty;
        public FileHash[] TileSetHashes { get; set; } = new FileHash[0];
        public FileHash[] ImageHashes { get; set; } = new FileHash[0];


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            MapData = deserializer.Read(MapData);
            TileSetHashes = deserializer.Read(TileSetHashes);
            ImageHashes = deserializer.Read(ImageHashes);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(MapData);
            serializer.Write(TileSetHashes);
            serializer.Write(ImageHashes);
        }
    }
}
