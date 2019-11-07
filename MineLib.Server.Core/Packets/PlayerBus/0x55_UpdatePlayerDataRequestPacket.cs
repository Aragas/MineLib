﻿using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Core;

namespace MineLib.Server.Core.Packets.PlayerHandler
{
    [Packet(0x55)]
    public sealed class UpdatePlayerDataRequestPacket : InternalPacket
    {
        public IPlayer Player;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Player = deserializer.Read(Player);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Player);
        }
    }
}