using System;
using Aragas.Network.Data;
using Aragas.Network.IO;
using MineLib.Protocol5.Data;

namespace MineLib.Protocol5.Packets.Client.Play
{
    public class SpawnPlayerPacket : ClientPlayPacket
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

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            EntityID = deserializer.Read(EntityID);
            PlayerUUID = deserializer.Read(PlayerUUID);
            PlayerName = deserializer.Read(PlayerName);
            Data = deserializer.Read(Data);
            X = deserializer.Read(X);
            Y = deserializer.Read(Y);
            Z = deserializer.Read(Z);
            Yaw = deserializer.Read(Yaw);
            Pitch = deserializer.Read(Pitch);
            HeadPitch = deserializer.Read(HeadPitch);
            CurrentItem = deserializer.Read(CurrentItem);
            Metadata = deserializer.Read(Metadata);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(EntityID);
            serializer.Write(PlayerUUID);
            serializer.Write(PlayerName);
            serializer.Write(Data);
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
            serializer.Write(HeadPitch);
            serializer.Write(CurrentItem);
            serializer.Write(Metadata);
        }
    }
}