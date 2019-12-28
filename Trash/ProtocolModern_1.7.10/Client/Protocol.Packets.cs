using Aragas.Core.PacketHandlers;
using Aragas.Core.Packets;

using MineLib.Core.Client;

using ProtocolModern.PacketHandlers;

namespace ProtocolModern.Client
{
    public sealed partial class Protocol
    {
        private void OnPacketHandled(int id, ProtobufPacket packet, ClientState state)
        {
            if(!Connected)
                return;

            ContextFunc<ProtobufPacket> handler = null;

            switch (state)
            {
                case ClientState.Joining:
                    handler = ServerPacketHandlers.LoginPacketResponses.Handlers[id]?.Invoke(this);
                    break;

                case ClientState.Joined:
                    handler = ServerPacketHandlers.PlayPacketResponses.Handlers[id]?.Invoke(this);
                    break;

                case ClientState.InfoRequest:
                    handler = ServerPacketHandlers.StatusPacketResponses.Handlers[id]?.Invoke(this);
                    break;
            }

            if (handler != null)
            {
                handler.SetContext(this);
                var response = handler.Handle(packet);
                if (response != null)
                    SendPacket(response);
            }

            DoCustomReceiving(packet);

            foreach (var modAPI in ModAPIs)
                modAPI.OnPacket(packet);
        }
    }
}