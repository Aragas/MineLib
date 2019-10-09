using System;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;

using ProtocolModern.Data;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client.Play
{
    public class SpawnPlayerPacket : ProtobufPacket
    {
        public VarInt EntityID;
        public String PlayerUUID;
        public String PlayerName;
        public PlayerData[] Data;
        public Int32 X;
        public Int32 Y;
        public Int32 Z;
        public SByte Yaw;
        public SByte Pitch;
        public SByte HeadPitch;
        public Int16 CurrentItem;
        public EntityMetadataList Metadata;

        public override VarInt ID => ClientPlayPacketTypes.SpawnPlayer;

        public override ProtobufPacket ReadPacket(ProtobufDataReader reader)
        {
            EntityID = reader.Read(EntityID);
            PlayerUUID = reader.Read(PlayerUUID);
            PlayerName = reader.Read(PlayerName);
            Data = reader.Read(Data);
            X = reader.Read(X);
            Y = reader.Read(Y);
            Z = reader.Read(Z);
            Yaw = reader.Read(Yaw);
            Pitch = reader.Read(Pitch);
            HeadPitch = reader.Read(HeadPitch);
            CurrentItem = reader.Read(CurrentItem);
            Metadata = reader.Read(Metadata);

            return this;
        }

        public override ProtobufPacket WritePacket(ProtobufStream stream)
        {
            stream.Write(EntityID);
            stream.Write(PlayerUUID);
            stream.Write(PlayerName);
            stream.Write(Data);
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Z);
            stream.Write(Yaw);
            stream.Write(Pitch);
            stream.Write(HeadPitch);
            stream.Write(CurrentItem);
            stream.Write(Metadata);

            return this;
        }

    }
}