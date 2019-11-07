using MineLib.Core;
using MineLib.Core.Anvil;

using System.Collections.Generic;
using System.Numerics;

namespace MineLib.Server.WorldBus.Generator
{
    public class FlatlandGenerator : IWorldGenerator
    {
        static FlatlandGenerator()
        {
            DefaultGeneratorOptions = "1;7,2x3,2;1";
        }

        public FlatlandGenerator()
        {
            GeneratorOptions = DefaultGeneratorOptions;
            SpawnPoint = new Vector3(0, 4, 0);
        }

        public FlatlandGenerator(string generatorOptions)
        {
            GeneratorOptions = generatorOptions;
        }

        public static string DefaultGeneratorOptions { get; set; }

        public string GeneratorOptions
        {
            get => generatorOptions;
            set
            {
                generatorOptions = value;
                CreateLayers();
            }
        }

        public ushort Biome { get; set; }

        protected List<GeneratorLayer> Layers;
        private string generatorOptions;

        public void Initialize(Level? level)
        {
            if (GeneratorOptions != null)
                return;
            if (level != null && !string.IsNullOrEmpty(level.GeneratorOptions))
                GeneratorOptions = level.GeneratorOptions;
            else
                GeneratorOptions = DefaultGeneratorOptions;
            if (string.IsNullOrEmpty(GeneratorOptions))
                GeneratorOptions = DefaultGeneratorOptions;
        }

        private void CreateLayers()
        {
            var parts = GeneratorOptions.Split(';');
            var layers = parts[1].Split(',');
            Layers = new List<GeneratorLayer>();
            double y = 0;
            foreach (var layer in layers)
            {
                var generatorLayer = new GeneratorLayer(layer);
                y += generatorLayer.Height;
                Layers.Add(generatorLayer);
            }
            Biome = ushort.Parse(parts[2]);
            SpawnPoint = new Vector3(0F, (float) y, 0F);
        }

        public Chunk GenerateChunk(Location2D location)
        {
            var chunk = new Chunk(location);
            int y = 0;
            for (int i = 0; i < Layers.Count; i++)
            {
                int height = y + Layers[i].Height;
                while (y < height)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        for (int z = 0; z < 16; z++)
                        {
                            chunk.SetBlock(new Location3D(location.X * Chunk.Width + x, y, location.Z * Chunk.Depth + z), new ReadonlyBlock32((ulong) Layers[i].BlockId, Layers[i].Metadata));
                        }
                    }
                    y++;
                }
            }
            for (int i = 0; i < chunk.Biomes.Length; i++)
                chunk.Biomes[i] = (byte) Biome;
            return chunk;
        }

        public string LevelType => "FLAT";

        public string GeneratorName => "FLAT";

        public long Seed { get; set; }

        public Vector3 SpawnPoint { get; set; }

        protected class GeneratorLayer
        {
            public GeneratorLayer(string layer)
            {
                var parts = layer.Split('x');
                int idIndex = 0;
                if (parts.Length == 2)
                    idIndex++;
                var idParts = parts[idIndex].Split(':');
                BlockId = short.Parse(idParts[0]);
                if (idParts.Length == 2)
                    Metadata = (byte)(byte.Parse(idParts[1]) & 0xF);
                Height = 1;
                if (parts.Length == 2)
                    Height = int.Parse(parts[0]);
            }

            public short BlockId { get; set; }
            public byte Metadata { get; set; }
            public int Height { get; set; }
        }
    }
}
