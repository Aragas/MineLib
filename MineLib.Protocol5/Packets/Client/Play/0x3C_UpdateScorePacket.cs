using System;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class UpdateScorePacket : ClientPlayPacket
    {
        public String ItemName;
        public SByte UpdateRemove;
        public String ScoreName;
        public Int32 Value;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            ItemName = deserialiser.Read(ItemName);
            UpdateRemove = deserialiser.Read(UpdateRemove);
            ScoreName = deserialiser.Read(ScoreName);
            Value = deserialiser.Read(Value);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(ItemName);
            serializer.Write(UpdateRemove);
            serializer.Write(ScoreName);
            serializer.Write(Value);
        }
    }
}