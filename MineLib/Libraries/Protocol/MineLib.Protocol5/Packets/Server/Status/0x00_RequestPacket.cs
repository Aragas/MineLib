using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Server.Status
{
    public class RequestPacket : ServerStatusPacket
    {
        public override void Deserialize(IPacketDeserializer deserialiser) { }

        public override void Serialize(IStreamSerializer serializer) { }
    }
}