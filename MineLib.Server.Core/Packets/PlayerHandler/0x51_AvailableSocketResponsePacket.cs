using System;
using System.Net;

using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Core.Packets.PlayerHandler
{
    [Packet(0x51)]
    public sealed class AvailableSocketResponsePacket : InternalPacket
    {
        public IPEndPoint Endpoint;
        public Boolean AvailableSocket;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Endpoint = deserializer.Read(Endpoint);
            AvailableSocket = deserializer.Read(AvailableSocket);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(Endpoint);
            serializer.Write(AvailableSocket);
        }
    }
}