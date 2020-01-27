using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Classic.Packets;

namespace ProtocolClassic.Packets.Server
{
    [PacketID(0x0E), PacketSize(65)]
    public class DisconnectPlayerPacket : ServerClassicPacket
    {
        public string Reason;

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