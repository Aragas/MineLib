using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server.Status
{
    public class RequestPacket : ProtobufPacket
    {
        public override VarInt ID => ServerStatusPacketTypes.Request;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
            return this;
        }

    }
}