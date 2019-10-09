using Aragas.Core.Packets;

using ProtocolModern.Packets.Client.Status;

namespace ProtocolModern.PacketHandlers.Server.Status
{
    public class ResponseHandler : ProtocolPacketHandler<ResponsePacket>
    {
        public override ProtobufPacket Handle(ResponsePacket packet)
        {
            return null;
        }
    }
}
