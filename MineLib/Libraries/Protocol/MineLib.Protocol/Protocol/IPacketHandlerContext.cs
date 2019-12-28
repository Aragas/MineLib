/*
using MineLib.Protocol.Packets;
using MineLib.Protocol.Server;

namespace MineLib.Protocol.Protocol
{
    public interface IPacketHandlerContext { }

    public abstract class PacketHandler { }
    public abstract class PacketHandler<TRequestPacket, TReplyPacket, TContext> : PacketHandler 
        where TRequestPacket : MinecraftPacket
        where TReplyPacket : MinecraftPacket where TContext : IPacketHandlerContext
    {
        public TContext Context { protected get; set; }

        public abstract TReplyPacket Handle(TRequestPacket packet);
    }

    public abstract class ProtocolPacketHandler<TRequestPacket> :
        PacketHandler<TRequestPacket, MinecraftPacket, ProtocolConnection> where TRequestPacket : MinecraftPacket
    { }

    public abstract class ProtocolPacketHandler<TRequestPacket, TResponsePacket> :
        PacketHandler<TRequestPacket, TResponsePacket, ProtocolConnection> where TRequestPacket : MinecraftPacket where TResponsePacket : MinecraftPacket
    { }
}
*/