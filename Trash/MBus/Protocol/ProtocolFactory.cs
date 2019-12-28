using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Protocol.IPC.Packets
{
    public class ProtocolFactory : DefaultPacketFactory<ProtocolPacket, VarInt, ProtobufSerializer, ProtobufDeserializer>
    {
        public static ProtocolPacket GetPacket(byte[] data)
        {
            using (var deserializer = new ProtobufDeserializer(data))
            {
                var dataLength = deserializer.Read<VarInt>();
                if (dataLength != 0)
                {
                    var id = deserializer.Read<VarInt>();
                    var packet = new ProtocolFactory().Create(id);
                    packet.Deserialize(deserializer);
                    return packet;
                }
                return null;
            }
        }
    }
}