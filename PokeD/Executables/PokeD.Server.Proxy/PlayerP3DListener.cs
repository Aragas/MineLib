using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.QServer.Core;

using PokeD.Server.Proxy.Packets.P3D;
using PokeD.Server.Proxy.Protocol.Factory;
using PokeD.Server.Proxy.Protocol.P3D;

namespace PokeD.Server.Proxy
{
    internal sealed class PlayerP3DListener : DefaultListener<PlayerP3DConnection, EmptyFactory, ProxyP3DTransmission, ProxyP3DPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public override int Port { get; } = 15124;
    }
}