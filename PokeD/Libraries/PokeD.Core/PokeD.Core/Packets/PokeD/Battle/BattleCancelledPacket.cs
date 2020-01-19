using Aragas.Network.IO;

namespace PokeD.Core.Packets.PokeD.Battle
{
    /// <summary>
    /// From Server
    /// </summary>
    public class BattleCancelledPacket : PokeDPacket
    {
        public string Reason { get; set; } = string.Empty;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Reason = deserializer.Read(Reason);
        }
        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Reason);
        }
    }
}
