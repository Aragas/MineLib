using Aragas.Network.IO;

namespace MineLib.Protocol.Netty.Packets.Server.Status
{
    public class RequestPacket : ServerStatusPacket
    {
        public override void Deserialize(ProtobufDeserializer deserialiser) { }

        public override void Serialize(ProtobufSerializer serializer) { }
    }
}