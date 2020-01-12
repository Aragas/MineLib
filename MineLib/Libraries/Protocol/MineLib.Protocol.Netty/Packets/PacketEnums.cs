namespace MineLib.Protocol.Netty.Packets
{
    public enum ServerHandshakePacketTypes
    {
        Handshake = 0x00,
        LegacyServerListPing = 0xFE
    }
}