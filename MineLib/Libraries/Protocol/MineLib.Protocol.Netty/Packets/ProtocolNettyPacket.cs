using System;
using System.Linq;

using Aragas.Network.Data;

using MineLib.Protocol.Packets;

namespace MineLib.Protocol.Netty.Packets
{
    public abstract class ProtocolNettyPacket<TEnum> : MinecraftPacket where TEnum : Enum
    {
        private static TEnum[] Cache { get; } = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();

        public override VarInt ID { get; }

        protected ProtocolNettyPacket() => ID = (VarInt) (dynamic) Cache.Single(@enum => GetType().Name == $"{@enum}Packet");
    }

    public abstract class ClientLoginPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : Enum { }
    public abstract class ClientPlayPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : Enum { }
    public abstract class ClientStatusPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : Enum { }

    public abstract class ServerHandshakePacket : ProtocolNettyPacket<ServerHandshakePacketTypes> { }
    public abstract class ServerLoginPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : Enum { }
    public abstract class ServerPlayPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : Enum { }
    public abstract class ServerStatusPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : Enum { }
}