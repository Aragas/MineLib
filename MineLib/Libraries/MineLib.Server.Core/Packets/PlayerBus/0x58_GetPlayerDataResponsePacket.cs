using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.QServer.Core.Packets;

using MineLib.Core;

namespace MineLib.Server.Core.Packets.PlayerHandler
{
    [Packet(0x58)]
    public sealed class GetPlayerDataResponsePacket : InternalPacket
    {
        public IPlayer? Player;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Player = deserializer.Read(Player);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Player);
        }
    }
}