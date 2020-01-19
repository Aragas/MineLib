using Aragas.Network.IO;

using PokeD.Core.Data.SCON;

namespace PokeD.Core.Packets.SCON.Status
{
    public class PlayerInfoListResponsePacket : SCONPacket
    {
        public PlayerInfo[] PlayerInfos { get; set; } = new PlayerInfo[0];


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PlayerInfos = deserializer.Read(PlayerInfos);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(PlayerInfos);
        }
    }
}
