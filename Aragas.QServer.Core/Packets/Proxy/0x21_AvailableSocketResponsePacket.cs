using Aragas.Network.Attributes;
using Aragas.Network.IO;

using System.Net;

namespace Aragas.QServer.Core.Packets.PlayerHandler
{
    [Packet(0x21)]
    public sealed class AvailableSocketResponsePacket : InternalPacket
    {
        public IPEndPoint Endpoint = new IPEndPoint(IPAddress.Any, 0);

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