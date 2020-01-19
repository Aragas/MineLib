using System;
using System.Linq;

using Aragas.Network.Data;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol575.Packets
{
    public abstract class Protocol575Packet<TEnum> : MinecraftEnumPacket where TEnum : Enum
    {
        private static TEnum[] Cache { get; } = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

        public override VarInt ID { get; }

        protected Protocol575Packet() => ID = (VarInt) (dynamic) Cache.Single(@enum => GetType().Name == $"{@enum}Packet");
    }

    public abstract class ClientLoginPacket : Protocol575Packet<ClientLoginPacketTypes> { }
    public abstract class ClientPlayPacket : Protocol575Packet<ClientPlayPacketTypes> { }
    public abstract class ClientStatusPacket : Protocol575Packet<ClientStatusPacketTypes> { }

    public abstract class ServerLoginPacket : Protocol575Packet<ServerLoginPacketTypes> { }
    public abstract class ServerPlayPacket : Protocol575Packet<ServerPlayPacketTypes> { }
    public abstract class ServerStatusPacket : Protocol575Packet<ServerStatusPacketTypes> { }
}
