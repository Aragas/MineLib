using System;

using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

using MineLib.Server.Proxy.Packets.Netty;

namespace MineLib.Server.Proxy.Protocol.Factory
{
    internal sealed class EmptyFactory : BasePacketFactory<ProxyNettyPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public override ProxyNettyPacket Create(VarInt packetID) => default;

        public override TPacketTypeCustom Create<TPacketTypeCustom>() => default;

        public override TPacketTypeCustom Create<TPacketTypeCustom>(Func<TPacketTypeCustom> initializer) => default;

        public override void Dispose() { }
    }
}