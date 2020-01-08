using Aragas.QServer.Core;

using PokeD.Core.IO;
using PokeD.Core.Packets.P3D;

using PokeD.Server.Proxy.Protocol.Factory;
using PokeD.Server.Proxy.Protocol.P3D;

namespace PokeD.Server.Proxy
{
    internal sealed class ProxyP3DListener : DefaultListener<ProxyP3DConnection, EmptyFactory, ProxyP3DTransmission, P3DPacket, int, P3DSerializer, P3DDeserializer>
    {
        public override int Port { get; } = 15124;
    }
}