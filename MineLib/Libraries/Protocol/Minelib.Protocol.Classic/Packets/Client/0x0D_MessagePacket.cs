using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Protocol.Classic.Packets.Client
{
    [PacketID(0x0D), PacketSize(65)]
    public class MessagePacket : ClientClassicPacket
    {
        public byte Unused;
        public string Message;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            Unused = deserializer.Read(Unused);
            Message = deserializer.Read(Message);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Unused);
            serializer.Write(Message);
        }
    }
}