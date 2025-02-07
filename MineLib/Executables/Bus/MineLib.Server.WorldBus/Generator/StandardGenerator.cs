﻿using MineLib.Core;
using MineLib.Core.Anvil;

using System;
using System.Numerics;

namespace MineLib.Server.WorldBus.Generator
{
    public enum Biome
    {
        Ocean = 0,
        Plains = 1,
        Desert = 2,
        ExtremeHills = 3,
        Forest = 4,
        Taiga = 5,
        Swampland = 6,
        River = 7,
        Hell = 8,
        Sky = 9,
        FrozenOcean = 10,
        FrozenRiver = 11,
        IcePlains = 12,
        IceMountains = 13,
        MushroomIsland = 14,
        MushroomIslandShore = 15,
        Beach = 16,
        DesertHills = 17,
        ForestHills = 18,
        TaigaHills = 19,
        ExtremeHillsEdge = 20,
        Jungle = 21,
        JungleHills = 22,
    }

    public class StandardGenerator : IWorldGenerator
    {
        public string LevelType => "DEFAULT";

        public string GeneratorName => "default";

        public string GeneratorOptions { get; set; }

        public long Seed { get; set; } = 123;

        public Vector3 SpawnPoint { get; set; }

        /// <summary>
        /// The noise generator used
        /// </summary>
        private Noise noise;
        /// <summary>
        /// The water level.
        /// </summary>
        private int waterLevel = 50;

        /// <summary>
        /// Generates a chunk by getting an array of heights then placing blocks of varying types up to that height
        /// then it adds trees (leaves first then trunk)
        /// 
        /// </summary>
        /// <returns>The chunk.</returns>
        /// <param name="location">Location</param>
        public Chunk GenerateChunk(Location2D location)
        {
            // TODO: Add Ores
            // TODO: Add Caves
            int trees = new Random().Next(0, 10);
            int[,] heights = new int[16, 16];
            int[,] treeBaseLocations = new int[trees, 2];

            for (int t = 0; t < trees; t++)
            {
                treeBaseLocations[t, 0] = new Random().Next(1, 16);
                treeBaseLocations[t, 1] = new Random().Next(1, 16);
            }

            //Make a new Chunk
            var chunk = new Chunk(location);
            //Loop through all the blocks in the chunk
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    int height = GetHeight((chunk.Location.X * Chunk.Width) + x, (chunk.Location.Z * Chunk.Depth) + z);
                    for (int y = 0; y < height; y++)
                    {
                        var worldLocation = new Location3D((location.X * Chunk.Width) + x, y, (location.Z * Chunk.Depth) + z);

                        if (y == 0) // if at the bottom then set block to bedrock
                            chunk.SetBlock(in worldLocation, new ReadonlyBlock32(7));
                        else if (y < height - 1) // if not at the top set the block to dirt or stone depending on height
                        {
                            if (!(y < (height / 4) * 3))
                                chunk.SetBlock(in worldLocation, new ReadonlyBlock32(3));
                            else
                                chunk.SetBlock(in worldLocation, new ReadonlyBlock32(1));
                        }
                        else if (y < waterLevel) // if below the water set to sand or clay
                        {
                            if (new Random().Next(1, 40) < 5 && y < waterLevel - 4)
                                chunk.SetBlock(in worldLocation, new ReadonlyBlock32(82));
                            else
                                chunk.SetBlock(in worldLocation, new ReadonlyBlock32(12));
                        }
                        else
                        {
                            // otherwise set the block to grass or gravel rarely
                            chunk.SetBlock(in worldLocation, new ReadonlyBlock32(2));
                        }
                        chunk.SetBlockBiome(new Location2D(x, z), (byte) Biome.ExtremeHills);
                        if (y < waterLevel + 17)
                            chunk.SetBlockBiome(new Location2D(x, z), (byte) Biome.ExtremeHillsEdge);
                        if (y < waterLevel + 10)
                            chunk.SetBlockBiome(new Location2D(x, z), (byte) Biome.Beach);

                        chunk.SetBlockLight(in worldLocation, 15);
                        chunk.SetBlockSkylight(in worldLocation, 15);
                        //if(y == height - 1)
                        //{
                        //    chunk.SetBlockSkylight(worldLocation, 15);
                        //}
                    }
                    heights[x, z] = height;

                    //create beaches and place water
                    if (height <= waterLevel)
                    {
                        for (int w = 0; w < waterLevel - 3; w++)
                        {
                            var worldLocation = new Location3D((location.X * Chunk.Width) + x, w, (location.Z * Chunk.Depth) + z);
                            if (chunk.GetBlock(in worldLocation).ID == 0)
                            {
                                chunk.SetBlock(in worldLocation, new ReadonlyBlock32(8));
                            }
                        }
                    }

                    // Generate colour of the wood and leaves
                    int woodColor = new Random().Next(1, 3);
                    if (woodColor == 1)
                        woodColor = 0;

                    // Generate trees
                    for (int pos = 0; pos < trees; pos++)
                    {
                        int random = new Random().Next(3, 4);
                        int treeBase = heights[treeBaseLocations[pos, 0], treeBaseLocations[pos, 1]];//chunk.GetHeight((byte)treeBaseLocations[pos, 0], (byte)treeBaseLocations[pos, 1]);
                        if (treeBaseLocations[pos, 0] < 14 && treeBaseLocations[pos, 0] > 4 && treeBaseLocations[pos, 1] < 14 && treeBaseLocations[pos, 1] > 4)
                        {
                            if (treeBase < waterLevel + 10)
                                break;
                            int leafwidth = 4;
                            for (int layer = 0; layer <= height; layer++)
                            {
                                for (int w = 0; w <= leafwidth; w++)
                                {
                                    for (int l = 0; l <= leafwidth; l++)
                                    {
                                        var worldLocation = new Location3D(
                                            (location.X * Chunk.Width) + treeBaseLocations[pos, 0] - (leafwidth / 2) + w,
                                            treeBase + layer + random,
                                            (location.Z * Chunk.Depth) + treeBaseLocations[pos, 1] - (leafwidth / 2) + l);
                                        chunk.SetBlock(worldLocation, new ReadonlyBlock32(18, (byte) woodColor));
                                    }
                                }
                                leafwidth -= 1;
                            }

                            for (int t = 0; t <= (random + 2); t++)
                            {
                                var worldLocation = new Location3D(
                                    (location.X * Chunk.Width) + treeBaseLocations[pos, 0],
                                    treeBase + t,
                                    (location.Z * Chunk.Depth) + treeBaseLocations[pos, 1]);
                                chunk.SetBlock(worldLocation, new ReadonlyBlock32(17, (byte) woodColor));
                            }
                        }
                    }
                }
            }

            return chunk;
        }

        /// <summary>
        /// Called after the world generator is created and
        /// all values are set.
        /// </summary>
        public void Initialize(Level? level)
        {
            const double persistence = 1, frequency = 0.01, amplitude = 80;
            int octaves = 2;
            noise = new Noise(persistence, frequency, amplitude, octaves, (int) Seed);
            SpawnPoint = new Vector3(0, GetHeight(0, 0), 0);
        }

        private int GetHeight(int x, int z)
        {
            var height = -1 * (int)noise.Get2D(x, z);
            if (height <= 0)
                height = height * -1 + 4;
            return height + 40;
        }
    }
}
