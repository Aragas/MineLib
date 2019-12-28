using System.Net.Sockets;

using Aragas.Network.Data;
using Aragas.Network.IO;

using MineLib.Protocol.Packets;
using MineLib.Protocol.Protocol;
using MineLib.Protocol5.Packets.Client.Login;
using MineLib.Protocol5.Packets.Server.Handshake;

namespace MineLib.Protocol5.Protocol
{
    public class Protocol5Transmission : MinecraftTransmission
    {
        public State State { get; set; }

        private HandshakeFactory HandshakeFactory { get; } = new HandshakeFactory();
        private StatusFactory StatusFactory { get; } = new StatusFactory();
        private LoginFactory LoginFactory { get; } = new LoginFactory();
        private PlayFactory PlayFactory { get; } = new PlayFactory();

        //public Protocol5Transmission(Socket socket, State state = State.Handshake) : base(socket) { State = state; }
        public Protocol5Transmission() : base() { }

        public override MinecraftPacket ReadPacket()
        {
            if (Socket.Available > 0)
            {
                using var deserializer = new ProtobufDeserializer(Stream);
                var id = deserializer.Read<VarInt>();
                MinecraftPacket packet;
                switch (State)
                {
                    case State.Handshake:
                        packet = HandshakeFactory.Create(id);
                        break;
                    case State.Status:
                        packet = StatusFactory.Create(id);
                        break;
                    case State.Login:
                        packet = LoginFactory.Create(id);
                        break;
                    case State.Play:
                        packet = PlayFactory.Create(id);
                        break;
                    default:
                        packet = null;
                        break;
                }

                if (packet != null)
                {
                    packet.Deserialize(deserializer);

                    if (packet is HandshakePacket handshakePacket)
                        State = (State) (byte) handshakePacket.NextState;

                    return packet;
                }
            }

            return null;
        }

        public override void SendPacket(MinecraftPacket packet)
        {
            base.SendPacket(packet);

            if (packet is LoginSuccessPacket)
                State = State.Play;
        }
    }
}