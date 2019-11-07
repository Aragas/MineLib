using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Server.Core.Packets;

using System.Net.Sockets;

namespace MineLib.Server.Core.Protocol
{
    public abstract class InternalConnectionHandler : DefaultConnectionHandler<InternalTransmission, InternalPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        protected InternalConnectionHandler() : base() { }
        protected InternalConnectionHandler(Socket socket, BasePacketFactory<InternalPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>? factory = null)
            : base(socket, factory ?? new InternalFactory()) { }
    }
}