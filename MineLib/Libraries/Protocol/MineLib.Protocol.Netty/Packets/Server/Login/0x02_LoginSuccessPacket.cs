using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Packets;

using System;

namespace MineLib.Protocol.Netty.Packets.Server.Login
{
    [Packet(0x02)]
    public class LoginSuccessPacket : MinecraftPacket
    {
        public String UUID;
        public String Username;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            UUID = deserializer.Read(UUID);
            Username = deserializer.Read(Username);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(UUID);
            serializer.Write(Username);
        }
    }
}