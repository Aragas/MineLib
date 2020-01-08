using Aragas.Network.Packets;
using Aragas.QServer.Core.Protocol;

using PokeD.Core.IO;
using PokeD.Core.Packets.P3D;

namespace PokeD.Server.Core.Protocol
{
    public class P3DINetworkBusTransmission : SocketPacketINetworkBusTransmission<P3DPacket, int, P3DSerializer, P3DDeserializer>
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        public P3DINetworkBusTransmission() : base()
        {
            Factory = new DefaultPacketFactory<P3DPacket, int, P3DSerializer, P3DDeserializer>();
        }
    }
}
