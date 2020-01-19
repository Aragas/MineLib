using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.QServer.Core.Packets;

using System;

namespace MineLib.Server.Core.Packets.PlayerHandler
{
    [Packet(0x54)]
    public sealed class DataPacket : InternalPacket
    {
        public Byte[] Data;

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