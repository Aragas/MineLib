using System.Net;

using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Core.Packets.PlayerHandler
{
    [Packet(0x51)]
    public sealed class AvailableSocketResponsePacket : InternalPacket
    {
        public IPEndPoint Endpoint;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Endpoint = deserializer.Read(Endpoint);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Endpoint);
        }
    }
}