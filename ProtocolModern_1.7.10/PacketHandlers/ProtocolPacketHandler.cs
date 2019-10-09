using Aragas.Core.PacketHandlers;
using Aragas.Core.Packets;

using ProtocolModern.Client;

namespace ProtocolModern.PacketHandlers
{
    public abstract class ProtocolPacketHandler<TRequestPacket> :
        PacketHandler<TRequestPacket, ProtobufPacket, Protocol> where TRequestPacket : ProtobufPacket { }

    public abstract class ProtocolPacketHandler<TRequestPacket, TResponsePacket> :
        PacketHandler<TRequestPacket, TResponsePacket, Protocol> where TRequestPacket : ProtobufPacket where TResponsePacket : ProtobufPacket { }
}
