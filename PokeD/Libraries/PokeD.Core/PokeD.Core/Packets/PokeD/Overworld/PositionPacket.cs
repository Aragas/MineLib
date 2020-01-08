using Aragas.Network.IO;

using PokeD.Core.Data;
using PokeD.Core.Data.PokeD.Structs;

namespace PokeD.Core.Packets.PokeD.Overworld
{
    public class PositionPacket : PokeDPacket
    {
        private MetaPosition Info { get; set; }

        public Vector3 Position { get => Info.Position; set => Info = new MetaPosition(value); }


        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            Info = deserializer.Read(Info);
        }
        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Info);
        }
    }
}
