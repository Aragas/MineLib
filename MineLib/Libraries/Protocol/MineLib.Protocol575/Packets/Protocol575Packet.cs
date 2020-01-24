using MineLib.Protocol.Packets;

namespace MineLib.Protocol575.Packets
{
    public abstract class ClientLoginPacket : MinecraftPacket { }
    public abstract class ClientPlayPacket : MinecraftPacket { }
    public abstract class ClientStatusPacket : MinecraftPacket { }

    public abstract class ServerLoginPacket : MinecraftPacket { }
    public abstract class ServerPlayPacket : MinecraftPacket { }
    public abstract class ServerStatusPacket : MinecraftPacket { }
}