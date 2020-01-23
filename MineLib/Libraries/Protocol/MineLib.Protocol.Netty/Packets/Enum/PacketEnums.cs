namespace MineLib.Protocol.Netty.Packets.Enum
{
    public abstract class ClientLoginPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : System.Enum { }
    public abstract class ClientPlayPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : System.Enum { }
    public abstract class ClientStatusPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : System.Enum { }

    public abstract class ServerLoginPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : System.Enum { }
    public abstract class ServerPlayPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : System.Enum { }
    public abstract class ServerStatusPacket<TEnum> : ProtocolNettyPacket<TEnum> where TEnum : System.Enum { }

    public enum ServerHandshakePacketTypes
    {
        Handshake = 0x00,
        LegacyServerListPing = 0xFE
    }
}