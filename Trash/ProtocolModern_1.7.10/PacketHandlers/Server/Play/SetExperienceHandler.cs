using Aragas.Core.Packets;

using MineLib.Core.Events.ReceiveEvents;

using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class SetExperienceHandler : ProtocolPacketHandler<SetExperiencePacket>
    {
        public override ProtobufPacket Handle(SetExperiencePacket packet)
        {
            Context.ClientReceiveEvent(new SetExperienceEvent(packet.ExperienceBar, packet.Level, packet.TotalExperience));

            return null;
        }
    }
}
