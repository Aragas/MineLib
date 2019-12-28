using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ScoreboardObjectivePacket : ClientPlayPacket
    {
        public String ObjectiveName;
        public String ObjectiveValue;
        public SByte CreateRemove;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            ObjectiveName = deserialiser.Read(ObjectiveName);
            ObjectiveValue = deserialiser.Read(ObjectiveValue);
            CreateRemove = deserialiser.Read(CreateRemove);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(ObjectiveName);
            serializer.Write(ObjectiveValue);
            serializer.Write(CreateRemove);
        }
    }
}