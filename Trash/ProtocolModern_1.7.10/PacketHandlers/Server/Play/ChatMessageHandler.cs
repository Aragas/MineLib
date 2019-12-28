using Aragas.Core.Packets;

using MineLib.Core.Events.ReceiveEvents;

using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class ChatMessageHandler : ProtocolPacketHandler<ChatMessagePacket>
    {
        public override ProtobufPacket Handle(ChatMessagePacket packet)
        {
            Context.ClientReceiveEvent(new ChatMessageEvent(packet.JSONData));

            return null;
        }
    }
}
