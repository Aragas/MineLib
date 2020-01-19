﻿using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Proxy.Protocol.Legacy.Packets
{
    [Packet(0xFF)]
    internal sealed class KickPacket : ProxyLegacyPacket
    {
        public string Message { get; set; }

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
            Message = deserialiser.Read(Message);
        }

        public override void Serialize(IStreamSerializer serializer)
        {
            serializer.Write(Message);
        }
    }
}