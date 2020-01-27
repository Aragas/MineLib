using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Netty.Packets.Server;
using MineLib.Server.Proxy.Data;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets
{
    [PacketID(0xFE)]
    internal sealed class ServerListPingPacket : ServerStatusPacket
    {
        public byte Payload { get; set; }
        public byte Identifier { get; set; }
        public string Message { get; set; } = default!;
        public byte ProtocolVersion { get; set; }
        public string Host { get; set; } = default!;
        public int Port { get; set; }

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            if (deserializer.BytesLeft > 0)
            {
                Payload = deserializer.Read(Payload);
                if (deserializer.BytesLeft > 0)
                {
                    Identifier = deserializer.Read(Identifier);
                    Message = deserializer.Read<UTF16BEString>(Message);
                    var dataLength = deserializer.Read<short>();
                    ProtocolVersion = deserializer.Read(ProtocolVersion);
                    Host = deserializer.Read<UTF16BEString>(Host);
                    Port = deserializer.Read(Port);
                }
            }
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Payload);
            serializer.Write(Identifier);
            serializer.Write(Message);
            serializer.Write(ProtocolVersion);
            serializer.Write(Host);
            serializer.Write(Port);
        }
    }
}