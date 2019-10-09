using Aragas.Network.Data;
using Aragas.Network.IO;

using MineLib.Server.Core.Packets;

namespace MineLib.Server.Core.Protocol
{
    public abstract class InternalListener<TConnection>: DefaultListener<TConnection, InternalFactory, InternalTransmission, InternalPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
        where TConnection : InternalConnectionHandler, new()
    {

    }
}