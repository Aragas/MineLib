using System;
using System.Linq;

using Aragas.Network.Data;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol5.Packets
{
    //public abstract class Protocol5Packet<TEnum> : PacketWithEnum<TEnum, VarInt, ProtobufSerializer, ProtobufDeserializer>
    //    where TEnum : Enum { }

    public abstract class Protocol5Packet<TEnum> : MinecraftPacket where TEnum : Enum
    {
        private static TEnum[] Cache { get; } = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

        public override VarInt ID { get; }

        protected Protocol5Packet() => ID = (VarInt) (dynamic) Cache.Single(@enum => GetType().Name == $"{@enum}Packet");
    }

    public abstract class ClientLoginPacket : Protocol5Packet<ClientLoginPacketTypes> { }
    public abstract class ClientPlayPacket : Protocol5Packet<ClientPlayPacketTypes> { }
    public abstract class ClientStatusPacket : Protocol5Packet<ClientStatusPacketTypes> { }

    public abstract class ServerHandshakePacket : Protocol5Packet<ServerHandshakePacketTypes> { }
    public abstract class ServerLoginPacket : Protocol5Packet<ServerLoginPacketTypes> { }
    public abstract class ServerPlayPacket : Protocol5Packet<ServerPlayPacketTypes> { }
    public abstract class ServerStatusPacket : Protocol5Packet<ServerStatusPacketTypes> { }
}
