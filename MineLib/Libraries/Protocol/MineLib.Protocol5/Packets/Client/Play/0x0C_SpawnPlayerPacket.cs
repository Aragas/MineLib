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

        public override void Deserialize(IPacketDeserializer deserialiser)
        {
            EntityID = deserialiser.Read(EntityID);
            PlayerUUID = deserialiser.Read(PlayerUUID);
            PlayerName = deserialiser.Read(PlayerName);
            Data = deserialiser.Read(Data);
            X = deserialiser.Read(X);
            Y = deserialiser.Read(Y);
            Z = deserialiser.Read(Z);
            Yaw = deserialiser.Read(Yaw);
            Pitch = deserialiser.Read(Pitch);
            HeadPitch = deserialiser.Read(HeadPitch);
            CurrentItem = deserialiser.Read(CurrentItem);
            Metadata = deserialiser.Read(Metadata);
        }

        public override void Serialize(IStreamSerializer serializer)
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