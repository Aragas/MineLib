using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class ScoreboardObjectivePacket : ClientPlayPacket
    {
        public String ObjectiveName;
        public String ObjectiveValue;
        public SByte CreateRemove;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            ObjectiveName = deserializer.Read(ObjectiveName);
            ObjectiveValue = deserializer.Read(ObjectiveValue);
            CreateRemove = deserializer.Read(CreateRemove);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ObjectiveName);
            serializer.Write(ObjectiveValue);
            serializer.Write(CreateRemove);
        }
    }
}