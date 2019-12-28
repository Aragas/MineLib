using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using PokeD.Server.Proxy.Packets.P3D;

using System;

namespace PokeD.Server.Proxy.Protocol.Factory
{
    internal sealed class EmptyFactory : BasePacketFactory<ProxyP3DPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public override ProxyP3DPacket Create(VarInt packetID) => default;

        public override TPacketTypeCustom Create<TPacketTypeCustom>() => default;

        public override TPacketTypeCustom Create<TPacketTypeCustom>(Func<TPacketTypeCustom> initializer) => default;
    }
}