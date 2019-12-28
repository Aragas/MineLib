using Aragas.Core.Packets;

using MineLib.Core.Events.ReceiveEvents;

using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class DisconnectHandler : ProtocolPacketHandler<DisconnectPacket>
    {
        public override ProtobufPacket Handle(DisconnectPacket packet)
        {
            Context.ClientReceiveEvent(new DisconnectEvent());
            Context.Disconnect();

            return null;
        }
    }
}
