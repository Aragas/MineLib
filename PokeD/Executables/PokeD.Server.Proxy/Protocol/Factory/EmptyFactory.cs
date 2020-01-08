using Aragas.Network.Packets;

using PokeD.Core.IO;
using PokeD.Core.Packets.P3D;

using System;

namespace PokeD.Server.Proxy.Protocol.Factory
{
    internal sealed class EmptyFactory : BasePacketFactory<P3DPacket, int, P3DSerializer, P3DDeserializer>
    {
        public override P3DPacket Create(int packetID) => default;

        public override TPacketTypeCustom Create<TPacketTypeCustom>() => default;

        public override TPacketTypeCustom Create<TPacketTypeCustom>(Func<TPacketTypeCustom> initializer) => default;
    }
}