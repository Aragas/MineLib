using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Events.ReceiveEvents;

using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class SpawnPositionHandler : ProtocolPacketHandler<SpawnPositionPacket>
    {
        public override ProtobufPacket Handle(SpawnPositionPacket packet)
        {
            Context.ClientReceiveEvent(new SpawnPointEvent(new Position(packet.X, packet.Y, packet.Z)));

            return null;
        }
    }
}
