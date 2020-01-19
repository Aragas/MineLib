using Aragas.Network.Data;

using MineLib.Protocol.Netty.Packets;
using MineLib.Protocol.Packets;

using System;
using System.Linq;

namespace MineLib.Protocol5.Packets
{
    public abstract class Protocol5Packet<TEnum> : MinecraftEnumPacket where TEnum : Enum
    {
        private static TEnum[] Cache { get; } = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

        public override VarInt ID { get; }

        protected Protocol5Packet() => ID = (VarInt) (dynamic) Cache.Single(@enum => GetType().Name == $"{@enum}Packet");
    }

    public abstract class ClientLoginPacket : ClientLoginPacket<ClientLoginPacketTypes> { }
    public abstract class ClientPlayPacket : ClientPlayPacket<ClientPlayPacketTypes> { }
    public abstract class ClientStatusPacket : ClientStatusPacket<ClientStatusPacketTypes> { }

    public abstract class ServerLoginPacket : ServerLoginPacket<ServerLoginPacketTypes> { }
    public abstract class ServerPlayPacket : ServerPlayPacket<ServerPlayPacketTypes> { }
    public abstract class ServerStatusPacket : ServerStatusPacket<ServerStatusPacketTypes> { }
}