using Aragas.Network.Data;
using Aragas.Network.IO;

using MineLib.Protocol.Packets;
using MineLib.Protocol.Protocol;
using MineLib.Protocol.Netty.Packets.Server.Handshake;

using System;

namespace MineLib.Protocol.Netty.Protocol
{
    public class ProtocolNettyTransmission<TStatusEnum, TLoginEnum, TPlayEnum> : MinecraftINetworkBusTransmission
        where TStatusEnum : Enum
        where TLoginEnum : Enum
        where TPlayEnum : Enum
    {
        public State State { get; set; }

        private HandshakeFactory HandshakeFactory { get; } = new HandshakeFactory();
        private StatusFactory<TStatusEnum> StatusFactory { get; } = new StatusFactory<TStatusEnum>();
        private LoginFactory<TLoginEnum> LoginFactory { get; } = new LoginFactory<TLoginEnum>();
        private PlayFactory<TPlayEnum> PlayFactory { get; } = new PlayFactory<TPlayEnum>();

        //public ProtocolNettyTransmission(Socket socket, State state = State.Handshake) : base(socket) { State = state; }
        public ProtocolNettyTransmission() : base() { }

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

            if (packet.ID == 0x02)
                State = State.Play;
        }
    }
}