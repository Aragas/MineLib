using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;
using Aragas.QServer.Core.Packets;

using System.Net.Sockets;

namespace Aragas.QServer.Core.Protocol
{
    public abstract class InternalConnectionHandler : DefaultConnectionHandler<InternalTransmission, InternalPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        protected InternalConnectionHandler() : base() { }
        protected InternalConnectionHandler(Socket socket, BasePacketFactory<InternalPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>? factory = null)
            : base(socket, factory ?? new InternalFactory()) { }
    }
}