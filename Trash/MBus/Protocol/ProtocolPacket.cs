using Aragas.Network.Data;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Protocol.IPC.Packets
{
    // No need for GUID.
    public abstract class ProtocolPacket : PacketWithAttribute<VarInt, ProtobufSerializer, ProtobufDeserializer> { }
}