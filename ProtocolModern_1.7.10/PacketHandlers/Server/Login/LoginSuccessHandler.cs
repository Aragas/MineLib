using Aragas.Core.Packets;

using MineLib.Core.Client;

using ProtocolModern.Packets.Client.Login;

namespace ProtocolModern.PacketHandlers.Server.Login
{
    public class LoginSuccessHandler : ProtocolPacketHandler<LoginSuccessPacket>
    {
        public override ProtobufPacket Handle(LoginSuccessPacket packet)
        {
            Context.SetState(ClientState.Joined);

            return null;
        }
    }
}
