using Aragas.Network.IO;

using PokeD.Core.Data.PokeD.Structs;

namespace PokeD.Core.Packets.PokeD.Battle
{
    /// <summary>
    /// From Client
    /// </summary>
    public class BattleItemPacket : PokeDPacket
    {
        private MetaItem Info { get; set; }

        public short Monster { get => Info.Monster; set => Info = new MetaItem(value, Item); }
        public short Item { get => Info.Item; set => Info = new MetaItem(Monster, value); }


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Info = deserializer.Read(Info);
        }
        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Info);
        }
    }
}
