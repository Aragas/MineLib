using Aragas.QServer.Core.Protocol;
using Aragas.QServer.NetworkBus;

using PokeD.Core.IO;
using PokeD.Core.Packets.P3D;

using System;

namespace PokeD.Server.Core.Protocol
{
    public class P3DINetworkBusTransmission : SocketPacketINetworkBusTransmission<P3DPacket, int, P3DSerializer, P3DDeserializer>
    {
        public P3DINetworkBusTransmission(IAsyncNetworkBus networkBus, Guid playerId) : base(networkBus, playerId) { }
    }
}
