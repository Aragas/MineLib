using Aragas.Network.Data;
using Aragas.Network.IO;

using MineLib.Protocol.Packets;
using MineLib.Protocol.Protocol;

using System;

namespace MineLib.Protocol.Netty.Protocol
{
    public class ProtocolNettyTransmission<TStatusEnum, TLoginEnum, TPlayEnum> : MinecraftINetworkBusTransmission
        where TStatusEnum : Enum
        where TLoginEnum : Enum
        where TPlayEnum : Enum
    {
        public State State { get; set; }

        private ServerHandshakeEnumFactory HandshakeFactory { get; } = new ServerHandshakeEnumFactory();
        private StatusEnumFactory<TStatusEnum> StatusFactory { get; } = new StatusEnumFactory<TStatusEnum>();
        private LoginEnumFactory<TLoginEnum> LoginFactory { get; } = new LoginEnumFactory<TLoginEnum>();
        private PlayEnumFactory<TPlayEnum> PlayFactory { get; } = new PlayEnumFactory<TPlayEnum>();

        public override MinecraftEnumPacket? ReadPacket()
        {
            var data = Receive(0);
            if (data.IsEmpty)
                return null;

            using var deserializer = new ProtobufDeserializer(data);
            var id = deserializer.Read<VarInt>();
            MinecraftEnumPacket? packet = State switch
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

                if (packet is Packets.Enum.HandshakePacket handshakePacket)
                    State = (State) (byte) handshakePacket.NextState;

                return packet;
            }

            return null;
        }

        public override void SendPacket(MinecraftEnumPacket packet)
        {
            base.SendPacket(packet);

            if (packet.ID == 0x02)
                State = State.Play;
        }
    }
}