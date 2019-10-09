using Aragas.Core.Data;
using Aragas.Core.Packets;

using MineLib.Core.Events.ReceiveEvents;

using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class PlayerPositionAndLookHandler : ProtocolPacketHandler<PlayerPositionAndLookPacket>
    {
        public override ProtobufPacket Handle(PlayerPositionAndLookPacket packet)
        {
            Context.ClientReceiveEvent(new PlayerPositionEvent(new Vector3(packet.X, packet.Y, packet.Z)));
            Context.ClientReceiveEvent(new PlayerLookEvent(new Vector3(packet.Yaw, packet.Pitch)));

            return null;
        }
    }
}
