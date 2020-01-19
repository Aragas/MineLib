using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.QServer.Core.Packets;

using System;

namespace MineLib.Server.Core.Packets.PlayerHandler
{
    [Packet(0x57)]
    public sealed class GetPlayerDataRequestPacket : InternalPacket
    {
        public String Username;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Username = deserializer.Read(Username);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Username);
        }
    }
}