using Aragas.Core.Packets;

using MineLib.Core.Events.ReceiveEvents;

using ProtocolModern.Packets.Client.Play;

namespace ProtocolModern.PacketHandlers.Server.Play
{
    public class UpdateHealthHandler : ProtocolPacketHandler<UpdateHealthPacket>
    {
        public override ProtobufPacket Handle(UpdateHealthPacket packet)
        {
            Context.ClientReceiveEvent(new UpdateHealthEvent(packet.Health));
            Context.ClientReceiveEvent(new UpdateFoodEvent(packet.Food, packet.FoodSaturation));

            return null;
        }
    }
}
