using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Core.Packets.WorldBus
{
    [Packet(0x34)]
    public sealed class ChunksInSquareRequest : InternalPacket
    {
        public int X, Z, Radius;
        public bool SendBulk;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            X = deserializer.Read(X);
            Z = deserializer.Read(Z);
            Radius = deserializer.Read(Radius);
            SendBulk = deserializer.Read(SendBulk);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(X);
            serializer.Write(Z);
            serializer.Write(Radius);
            serializer.Write(SendBulk);
        }
    }
}