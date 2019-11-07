﻿using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Core.Packets.PlayerHandler
{
    [Packet(0x56)]
    public sealed class UpdatePlayerDataResponsePacket : InternalPacket
    {
        public int ErrorEnum;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            ErrorEnum = deserializer.Read(ErrorEnum);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(ErrorEnum);
        }
    }
}