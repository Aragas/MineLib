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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            ItemName = deserializer.Read(ItemName);
            UpdateRemove = deserializer.Read(UpdateRemove);
            ScoreName = deserializer.Read(ScoreName);
            Value = deserializer.Read(Value);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(ItemName);
            serializer.Write(UpdateRemove);
            serializer.Write(ScoreName);
            serializer.Write(Value);
        }
    }
}