﻿using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.QServer.Core.Packets;

namespace MineLib.Server.Core.Packets.EntityBus
{
    [PacketID(0x71)]
    public sealed class PlayerListResponse : InternalPacket
    {
        public string[] PlayerNames;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            PlayerNames = deserializer.Read(PlayerNames);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(PlayerNames);
        }
    }
}