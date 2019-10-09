using System;
using Aragas.Network.Data;
using Aragas.Network.IO;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class BlockBreakAnimationPacket : ClientPlayPacket
    {
        public VarInt EntityID;
        public Int32 X;
        public Int16 Y;
        public Int32 Z;
        public SByte DestroyStage;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            EntityID = deserialiser.Read(EntityID);
            X = deserialiser.Read(X);
            Y = deserialiser.Read(Y);
            Z = deserialiser.Read(Z);
            DestroyStage = deserialiser.Read(DestroyStage);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(DestroyStage);
        }
    }
}