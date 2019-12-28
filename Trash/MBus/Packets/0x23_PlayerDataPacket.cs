using Aragas.Network.Attributes;
using Aragas.Network.IO;
using Aragas.Network.Packets;

namespace MineLib.Protocol.IPC.Packets
{
    public enum Abilities
    {
        GodMode = 8,
        CanFly = 4,
        IsFlying = 2,
        Creative = 1
    }

    [Packet(0x23)]
    public class PlayerDataPacket : ProtocolPacket
    {
        /*
        public class ClientSettings
        {
            public string? Locale;
            public byte? ViewDistance;
            public byte? ChatMode;
            public bool? ChatColours;
            public byte? Difficulty;
            public bool? ShowCape;
            public byte? DisplayedSkinParts;
            public byte? MainHand;
        }
        */

        public byte? Animation;
        public Abilities? Abilities;
        public float? FlyingSpeed, WalkingSpeed;
        public double? X, FeetY, Z;
        public double? Yaw, Pitch;
        public bool? OnGround;
        public byte? HeldItem;
        public byte? Status;
        //public ClientSettings? Settings;
        public string Locale;
        public byte? ViewDistance;
        public byte? ChatMode;
        public bool? ChatColours;
        public byte? Difficulty;
        public bool? ShowCape;
        public byte? DisplayedSkinParts;
        public byte? MainHand;

        public override void Deserialize(ProtobufDeserializer deserialiser)
        {
            Animation = deserialiser.Read(Animation);
            Abilities = deserialiser.Read(Abilities);
            FlyingSpeed = deserialiser.Read(FlyingSpeed);
            WalkingSpeed = deserialiser.Read(WalkingSpeed);
            X = deserialiser.Read(X);
            FeetY = deserialiser.Read(FeetY);
            Z = deserialiser.Read(Z);
            Yaw = deserialiser.Read(Yaw);
            Pitch = deserialiser.Read(Pitch);
            OnGround = deserialiser.Read(OnGround);
            HeldItem = deserialiser.Read(HeldItem);
            Status = deserialiser.Read(Status);
            Locale = deserialiser.Read(Locale);
            ViewDistance = deserialiser.Read(ViewDistance);
            ChatMode = deserialiser.Read(ChatMode);
            ChatColours = deserialiser.Read(ChatColours);
            Difficulty = deserialiser.Read(Difficulty);
            ShowCape = deserialiser.Read(ShowCape);
            DisplayedSkinParts = deserialiser.Read(DisplayedSkinParts);
            MainHand = deserialiser.Read(MainHand);
        }

        public override void Serialize(ProtobufSerializer serializer)
        {
            serializer.Write(Animation);
            serializer.Write(Abilities);
            serializer.Write(FlyingSpeed);
            serializer.Write(WalkingSpeed);
            serializer.Write(X);
            serializer.Write(FeetY);
            serializer.Write(Z);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
            serializer.Write(OnGround);
            serializer.Write(HeldItem);
            serializer.Write(Status);
            serializer.Write(Locale);
            serializer.Write(ViewDistance);
            serializer.Write(ChatMode);
            serializer.Write(ChatColours);
            serializer.Write(Difficulty);
            serializer.Write(ShowCape);
            serializer.Write(DisplayedSkinParts);
            serializer.Write(MainHand);
        }
    }
}