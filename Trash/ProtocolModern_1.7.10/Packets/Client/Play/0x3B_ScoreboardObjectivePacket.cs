using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class ScoreboardObjectivePacket : ProtobufPacket
    {
        public String ObjectiveName;
        public String ObjectiveValue;
        public SByte CreateRemove;

        public override VarInt ID => ClientPlayPacketTypes.ScoreboardObjective;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
            ObjectiveName = reader.Read(ObjectiveName);
            ObjectiveValue = reader.Read(ObjectiveValue);
            CreateRemove = reader.Read(CreateRemove);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
            stream.Write(ObjectiveName);
            stream.Write(ObjectiveValue);
            stream.Write(CreateRemove);

            return this;
        }

    }
}