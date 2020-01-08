using Aragas.Network.Data;
using Aragas.Network.IO;

using MineLib.Protocol.Packets;
using MineLib.Protocol.Protocol;
using MineLib.Protocol5.Packets.Client.Login;
using MineLib.Protocol5.Packets.Server.Handshake;

namespace MineLib.Protocol5.Protocol
{
    public class Protocol5Transmission : MinecraftINetworkBusTransmission
    {
        public State State { get; set; }

        private HandshakeFactory HandshakeFactory { get; } = new HandshakeFactory();
        private StatusFactory StatusFactory { get; } = new StatusFactory();
        private LoginFactory LoginFactory { get; } = new LoginFactory();
        private PlayFactory PlayFactory { get; } = new PlayFactory();

        //public Protocol5Transmission(Socket socket, State state = State.Handshake) : base(socket) { State = state; }
        public Protocol5Transmission() : base() { }

        public override MinecraftPacket? ReadPacket()
        {
            var data = Receive(0);
            if (data.IsEmpty)
                return null;

            using var deserializer = new ProtobufDeserializer(data);
            var id = deserializer.Read<VarInt>();
            MinecraftPacket? packet = State switch
            {
                State.Handshake => HandshakeFactory.Create(id),
                State.Status => StatusFactory.Create(id),
                State.Login => LoginFactory.Create(id),
                State.Play => PlayFactory.Create(id),
                _ => null,
            };
            if (packet != null)
            {
                packet.Deserialize(deserializer);

                if (packet is HandshakePacket handshakePacket)
                    State = (State) (byte) handshakePacket.NextState;

                return packet;
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