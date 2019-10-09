using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class UpdateScorePacket : ProtobufPacket
    {
        public String ItemName;
        public SByte UpdateRemove;
        public String ScoreName;
        public Int32 Value;

        public override VarInt ID => ClientPlayPacketTypes.UpdateScore;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
            ItemName = reader.Read(ItemName);
            UpdateRemove = reader.Read(UpdateRemove);
            ScoreName = reader.Read(ScoreName);
            Value = reader.Read(Value);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
            stream.Write(ItemName);
            stream.Write(UpdateRemove);
            stream.Write(ScoreName);
            stream.Write(Value);

            return this;
        }

    }
}