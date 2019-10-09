using System.Net.Sockets;

using Aragas.Network.Data;
using Aragas.Network.IO;

using MineLib.Server.Core;
using MineLib.Server.Core.Packets;
using MineLib.Server.Core.Protocol;

namespace MineLib.AddressTracker
{
    public abstract class Connection : StandardInternalConnection<InternalTransmission, InternalPacket, VarInt, ProtobufSerializer, ProtobufDeserialiser>
    {
        protected Connection(Socket socket) : base(socket)
        {
        }
    }
}