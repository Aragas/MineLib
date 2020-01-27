namespace MineLib.Protocol.Classic.Packets
{
    public abstract class ClassicPacket : PacketWithIDAndSizeAttribute<byte> { }

    public abstract class ClientClassicPacket : ClassicPacket { }
    public abstract class ServerClassicPacket : ClassicPacket { }
}