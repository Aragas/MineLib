using Aragas.Core.Packets;

using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class PluginMessageHandler : ProtocolPacketHandler<PluginMessagePacket>
    {
        public override ProtobufPacket Handle(PluginMessagePacket packet)
        {
            Context.OnPluginChannelMessage(packet.Channel, packet.Data);

            return null;
        }
    }
}
