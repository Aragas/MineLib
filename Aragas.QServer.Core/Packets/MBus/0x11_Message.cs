using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System;

namespace Aragas.QServer.Core.Packets.MBus
{
    [Packet(0x11)]
    public sealed class Message : InternalPacket
    {
        public byte[] Data = Array.Empty<byte>();

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Data = deserializer.Read(Data);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Data);
        }
    }
}