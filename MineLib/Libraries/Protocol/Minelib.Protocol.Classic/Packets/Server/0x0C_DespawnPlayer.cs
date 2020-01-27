using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x0C), PacketSize(2)]
    public class DespawnPlayerPacket : ServerClassicPacket
    {
        public sbyte PlayerID;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            PlayerID = deserializer.Read(PlayerID);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(PlayerID);
        }
    }
}