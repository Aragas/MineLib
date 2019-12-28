using System;

using Aragas.Core.Packets;

using MineLib.Core.Events.ReceiveEvents;

using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class TimeUpdateHandler : ProtocolPacketHandler<TimeUpdatePacket>
    {
        public override ProtobufPacket Handle(TimeUpdatePacket packet)
        {
            Context.ClientReceiveEvent(new TimeUpdateEvent(new TimeSpan((int)((packet.TimeOfDay / 1000 + 8) % 24), (int)(60 * (packet.TimeOfDay % 1000) / 1000), 0)));

            return null;
        }
    }
}
