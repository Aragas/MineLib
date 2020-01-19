using Aragas.Network.Attributes;
using Aragas.Network.IO;

using MineLib.Protocol.Netty.Packets.Server;
using MineLib.Server.Proxy.Data;

namespace MineLib.Server.Proxy.Protocol.Netty.Packets
{
    [Packet(0xFE)]
    internal sealed class ServerListPingPacket : ServerStatusPacket
    {
        public byte Payload { get; set; }
        public byte Identifier { get; set; }
        public string Message { get; set; }
        public byte ProtocolVersion { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
            if (deserialiser.BytesLeft() > 0)
            {
                Payload = deserialiser.Read(Payload);
                if (deserialiser.BytesLeft() > 0)
                {
                    Identifier = deserialiser.Read(Identifier);
                    Message = deserialiser.Read<UTF16BEString>(Message);
                    var dataLength = deserialiser.Read<short>();
                    ProtocolVersion = deserialiser.Read(ProtocolVersion);
                    Host = deserialiser.Read<UTF16BEString>(Host);
                    Port = deserialiser.Read(Port);
                }
            }
        }

        public override void Serialize(IStreamSerializer serializer)
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