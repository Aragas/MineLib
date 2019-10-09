using Aragas.Network.Attributes;
using Aragas.Network.IO;

namespace MineLib.Server.Core.Packets.EntityBus
{
    [Packet(0x71)]
    public sealed class PlayerListResponse : InternalPacket
    {
        public string[] PlayerNames;

        public override void Deserialize(ProtobufDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            PlayerNames = deserializer.Read(PlayerNames);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            base.Serialize(serializer);
            serializer.Write(PlayerNames);
        }
    }
}