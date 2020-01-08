using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Overworld.Map
{
    public class TileSetResponse
    {
        public string Name { get; set; } = string.Empty;
        public string TileSetData { get; set; } = string.Empty;
    }
    public class ImageResponse
    {
        public string Name { get; set; } = string.Empty;
        public byte[] ImageData { get; set; } = new byte[0];
    }

    public class TileSetResponsePacket : PokeDPacket
    {
        public TileSetResponse[] TileSets { get; set; } = new TileSetResponse[0];
        public ImageResponse[] Images { get; set; } = new ImageResponse[0];


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            TileSets = deserializer.Read(TileSets);
            Images = deserializer.Read(Images);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(TileSets);
            serializer.Write(Images);
        }
    }
}
