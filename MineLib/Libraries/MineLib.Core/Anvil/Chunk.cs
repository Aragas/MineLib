using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MineLib.Core.Anvil
{
    // 64b
    // 256b
    // 984576b
    // Total - 984896b              -> 962kb
    // ReadonlyBlockList            -> 896kb
    // ReadonlyBlockWithLightList   -> 960kb
    // ReadonlyBlock16              -> 1536kb
    // ReadonlyBlock32              -> 2048kb
    public readonly struct Chunk
    {
        public const ushort Width = 16;
        public const ushort Depth = 16;
        public const ushort SectionCount = 16;

        public readonly Location2D Location;
        public readonly byte[] Biomes;
        public readonly Section[] Sections;
        public bool IsEmpty => Sections.All(s => s.IsEmpty);

        public Chunk(in Location2D location)
        {
            Location = location;
            Biomes = new byte[Width * Depth];

            Sections = new Section[SectionCount];
            for (var i = 0; i < Sections.Length; i++)
                Sections[i] = Section.BuildEmpty(new Location3D(Location.X, i, Location.Z));
        }

        public ReadonlyBlock32 GetBlock(in Location3D blockWorldLocation) => GetSectionByY(blockWorldLocation.Y).GetBlock(GetLocationInSection(in blockWorldLocation));
        public void SetBlock(in Location3D blockWorldLocation, in ReadonlyBlock32 block) => GetSectionByY(blockWorldLocation.Y).SetBlock(GetLocationInSection(in blockWorldLocation), in block);

        //public void SetBlockMultiBlock(in Location3D blockWorldLocation, in ReadonlyBlock32 block) => GetSectionByY(blockWorldLocation.Y).SetBlock(new Location3D(blockWorldLocation.X, GetYinSection(blockWorldLocation.Y), blockWorldLocation.Z), in block);

        public byte GetBlockLight(in Location3D blockWorldLocation) => GetSectionByY(blockWorldLocation.Y).GetBlockLighting(GetLocationInSection(in blockWorldLocation));
        public void SetBlockLight(in Location3D blockWorldLocation, byte light) => GetSectionByY(blockWorldLocation.Y).SetBlockLighting(GetLocationInSection(in blockWorldLocation), light);

        public byte GetBlockSkylight(in Location3D blockWorldLocation) => GetSectionByY(blockWorldLocation.Y).GetBlockSkylight(GetLocationInSection(in blockWorldLocation));
        public void SetBlockSkylight(in Location3D blockWorldLocation, byte light) => GetSectionByY(blockWorldLocation.Y).SetBlockSkylight(GetLocationInSection(in blockWorldLocation), light);

        public byte GetBlockBiome(in Location3D blockWorldLocation) => GetBlockBiome(GetChunkLocation(in blockWorldLocation));
        public void SetBlockBiome(in Location3D blockWorldLocation, byte biome) => SetBlockBiome(GetChunkLocation(in blockWorldLocation), biome);

        public byte GetBlockBiome(in Location2D blockWorldLocation) => Biomes[(blockWorldLocation.Z * 16) + blockWorldLocation.X];
        public void SetBlockBiome(in Location2D blockWorldLocation, byte biome) => Biomes[(blockWorldLocation.Z * 16) + blockWorldLocation.X] = biome;

        #region Helping Methods

        private static int GetSectionXByX(int blockLocationX) => (int) Math.Floor((double) blockLocationX / (double) Width); // Should be equal to Coordinates.X
        private static int GetSectionYByY(int blockLocationY) => blockLocationY / SectionCount;
        private static int GetSectionZByZ(int blockLocationZ) => (int) Math.Floor((double) blockLocationZ / (double) Depth); // Should be equal to Coordinates.Z

        //private static Coordinates2D GetChunkCoordinates(in Location3D blockWorldLocation) => new Location2D(blockWorldLocation.X >> 4, blockWorldLocation.Z >> 4);
        public static Location2D GetChunkLocation(in Location3D blockWorldLocation) => new Location2D(
            GetSectionXByX(blockWorldLocation.X),
            GetSectionZByZ(blockWorldLocation.Z));

        private Section GetSectionByY(int blockLocationY) => Sections[GetSectionYByY(blockLocationY)];

        public static Location3D GetSectionLocation(in Location3D blockWorldLocation) => new Location3D(
            GetSectionXByX(blockWorldLocation.X),
            GetSectionYByY(blockWorldLocation.Y),
            GetSectionZByZ(blockWorldLocation.Z));
        public static Location3D GetLocationInSection(in Location3D blockWorldLocation)
        {
            var chunkLocation = GetChunkLocation(in blockWorldLocation);
            return new Location3D(
                GetXinSection(blockWorldLocation.X, in chunkLocation),
                GetYInSection(blockWorldLocation.Y),
                GetZinSection(blockWorldLocation.Z, in chunkLocation));
        }

        private static int GetXinSection(int blockWorldLocationX, in Location2D chunkLocation) => (blockWorldLocationX - (chunkLocation.X * Width)) % Width;
        private static int GetYInSection(int blockWorldLocationX) => blockWorldLocationX % 16;
        private static int GetZinSection(int blockWorldLocationZ, in Location2D chunkCoordinates) => (blockWorldLocationZ - (chunkCoordinates.Z * Depth)) % Depth;

        private int GetFilledSectionsCount() => Sections.Count(s => !s.IsEmpty);

        #endregion

        public override string ToString() => $"Filled Sections: {GetFilledSectionsCount()}";

        public static bool operator ==(Chunk left, Chunk right) => left.Location == right.Location;
        public static bool operator !=(Chunk left, Chunk right) => !(left == right);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Chunk chunk && Equals(chunk);
        public bool Equals(Chunk other) => other.Location.Equals(Location);

        public override int GetHashCode() => Location.GetHashCode();
    }
}