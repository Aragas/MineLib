using Aragas.Core.Packets;

using MineLib.Core.Events.ReceiveEvents;

using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class JoinGameHandler : ProtocolPacketHandler<JoinGamePacket>
    {
        public override ProtobufPacket Handle(JoinGamePacket packet)
        {
            Context.ClientReceiveEvent(new PlayerIDEvent(packet.EntityID));
            Context.ClientReceiveEvent(new GameInfoEvent(packet.GameMode, packet.Difficulty, (byte) packet.Dimension));

            return null;
        }
    }
}
