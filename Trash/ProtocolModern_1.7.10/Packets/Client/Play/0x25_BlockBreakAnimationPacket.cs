using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class BlockBreakAnimationPacket : ProtobufPacket
    {
        public VarInt EntityID;
        public Int32 X;
        public Int16 Y;
        public Int32 Z;
        public SByte DestroyStage;

        public override VarInt ID => ClientPlayPacketTypes.BlockBreakAnimation;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
            EntityID = reader.Read(EntityID);
            X = reader.Read(X);
            Y = reader.Read(Y);
            Z = reader.Read(Z);
            DestroyStage = reader.Read(DestroyStage);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
            stream.Write(EntityID);
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Z);
            stream.Write(DestroyStage);

            return this;
        }

    }
}