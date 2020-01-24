using System;
using System.IO;
using Aragas.Network.Attributes;
using Aragas.Network.Data;
using Aragas.Network.IO;
using fNbt;
using MineLib.Core;

namespace MineLib.Protocol575.Packets.Client.Play
{
    [Packet(0x24)]
    public class UpdateLightPacket : ClientPlayPacket
    {
        public VarInt X;
        public VarInt Z;

        public VarInt SkyLightMask;
        public VarInt BlockLightMask;
        public VarInt EmptySkyLightMask;
        public VarInt EmptyBlockLightMask;

        public byte[] Data;


        public override void Deserialize(IPacketDeserializer deserializer)
        {
            X = deserializer.Read(X);
            Z = deserializer.Read(Z);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Z);

            serializer.Write(SkyLightMask);
            serializer.Write(BlockLightMask);
            serializer.Write(EmptySkyLightMask);
            serializer.Write(EmptyBlockLightMask);

            serializer.Write(Data, false);
        }
    }

    [Packet(0x40)]
    public class UpdateViewPositionPacket : ClientPlayPacket
    {
        public VarInt X;
        public VarInt Z;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            X = deserializer.Read(X);
            Z = deserializer.Read(Z);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Z);
        }
    }

    [Packet(0x35)]
    public class PlayerPositionAndLookPacket : ClientPlayPacket
    {
        public Double X;
        public Double Y;
        public Double Z;
        public Single Yaw;
        public Single Pitch;
        public byte Flags;
        public VarInt TeleportId;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            X = deserializer.Read(X);
            Y = deserializer.Read(Y);
            Z = deserializer.Read(Z);
            Yaw = deserializer.Read(Yaw);
            Pitch = deserializer.Read(Pitch);
            Flags = deserializer.Read(Flags);
            TeleportId = deserializer.Read(TeleportId);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Y);
            serializer.Write(Z);
            serializer.Write(Yaw);
            serializer.Write(Pitch);
            serializer.Write(Flags);
            serializer.Write(TeleportId);
        }
    }

    [Packet(0x3D)]
    public class WorldBorderPacket : ClientPlayPacket
    {
        public Double X;
        public Double Z;
        public Double OldDiameter;
        public Double NewDiameter;
        public VarLong Speed;
        public VarInt PortalTeleportBoundary;
        public VarInt WarningTime;
        public VarInt WarningBlocks;

        public override void Deserialize(IPacketDeserializer deserializer)
        {

        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(new VarInt(3));
            serializer.Write(X);
            serializer.Write(Z);
            serializer.Write(OldDiameter);
            serializer.Write(NewDiameter);
            serializer.Write(Speed);
            serializer.Write(PortalTeleportBoundary);
            serializer.Write(WarningTime);
            serializer.Write(WarningBlocks);
        }
    }


    [Packet(0x21)]
    public class ChunkDataPacket : ClientPlayPacket
    {
        public Int32 X;
        public Int32 Z;
        public Boolean GroundUp;
        public VarInt BitMap;
        public NbtCompound Nbt;
        public int[] Biomes;
        public int DataLength;
        public byte[] ChunkData;
        public NbtCompound BlockEntities;

        public override void Deserialize(IPacketDeserializer deserializer)
        {
            X = deserializer.Read(X);
            Z = deserializer.Read(Z);
            GroundUp = deserializer.Read(GroundUp);
            BitMap = deserializer.Read(BitMap);
            Nbt = deserializer.Read(Nbt);
            Biomes = deserializer.Read(Biomes);
            ChunkData = deserializer.Read(ChunkData);
            //BlockEntities = deserializer.Read(BlockEntities);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(X);
            serializer.Write(Z);
            serializer.Write(GroundUp);
            serializer.Write(BitMap);
            using var ms = new MemoryStream();
            new NbtFile(Nbt).SaveToStream(ms, NbtCompression.None);
            var nbtData = ms.ToArray();
            serializer.Write(nbtData, false);
            //serializer.Write(new VarLong(0));
            serializer.Write(new VarInt(ChunkData.Length + Biomes.Length * 4));
            serializer.Write(ChunkData, false);
            serializer.Write(Biomes, false);
            serializer.Write(new VarInt(0));
            //serializer.Write(BlockEntities);
        }
    }

    [Packet(0x4D)]
    public class SpawnPositionPacket : ClientPlayPacket
    {
        public Location3D Location;
		//public Int32 X;
		//public Int32 Y;
		//public Int32 Z;

        public override void Deserialize(IPacketDeserializer deserializer)
        {

			//X = deserializer.Read(X);
			//Y = deserializer.Read(Y);
			//Z = deserializer.Read(Z);
        }

        public override void Serialize(IPacketSerializer serializer)
        {
            serializer.Write(Location.ToLong());
            //serializer.Write(X);
            //serializer.Write(Y);
            //serializer.Write(Z);
        }
    }
}