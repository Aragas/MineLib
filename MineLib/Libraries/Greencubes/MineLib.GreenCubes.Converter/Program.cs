using fNbt;
using MineLib.Core;
using MineLib.Core.Anvil;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace MineLib.GreenCubes.Converter
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            /*
            var nbt = new NbtFile("D:\\clotvostok v.2\\level.dat");
            var rootTag = nbt.RootTag;
            var tags = rootTag.Tags.Cast<NbtCompound>().First().Tags;
            */

            var list = new List<Chunk>();

            using var fs = new FileStream("D:\\clotvostok v.2\\region\\r.0.0.mcr", FileMode.Open, FileAccess.Read);
            using var br = new BinaryReader(fs);

            var locationTable = new List<(int Offset, int SectorCount)>();
            for (int i = 0; i < 1024; i++)
                locationTable.Add((br.ReadByte() << 16 | br.ReadByte() << 8 | br.ReadByte(), br.ReadByte()));

            var timestampTable = new List<int>();
            for (int i = 0; i < 1024; i++)
                timestampTable.Add(br.ReadInt32());


            foreach (var (Offset, SectorCount) in locationTable)
            {
                if (Offset == 0 && SectorCount == 0)
                    continue;

                fs.Seek(Offset * 4096, SeekOrigin.Begin);
                var length = br.ReadInt32();
                var compressionType = br.ReadByte();
                var nbt = new NbtFile();
                nbt.LoadFromStream(fs, NbtCompression.AutoDetect, null);

                var nbtTags = nbt.RootTag.Tags.Cast<NbtCompound>().First();

                var blocks = nbtTags["Blocks"].ByteArrayValue;
                var meta = nbtTags["Metadata"].ByteArrayValue;
                var chunk = new Chunk(new Location2D(nbtTags["xPos"].IntValue, nbtTags["zPos"].IntValue));

                /*
                var biomes = nbtTags["BiomeData"].ByteArrayValue;
                for (var i = 0; i < 256; i++)
                    chunk.Biomes[i] = biomes[i];
                */

                var index = 0;
                for (var x = 0; x < 16; x++)
                    for (var z = 0; z < 16; z++)
                        for (var y = 0; y < 128; y++)
                        {
                            chunk.SetBlock(new Location3D(x, y, z), new ReadonlyBlock32(blocks[index], meta[index++]));
                        }
                list.Add(chunk);
            }
            ;


            /*
            var length = br.ReadInt32();
            var compressionType = br.ReadByte();
            if (compressionType == 2)
            {
                br.ReadByte();
                br.ReadByte();

                var ms = new MemoryStream();
                using var decStream = new DeflateStream(fs, CompressionMode.Decompress, true);
                decStream.CopyTo(ms);

                //var nbt1 = new NbtFile();
                //nbt1.LoadFromStream(fs, NbtCompression.AutoDetect);
                //list.Add(nbt1);
            }
            */

            ;
            //nbt00.LoadFromStream(fs, NbtCompression.AutoDetect);
            //var nbt00Tags = nbt00.RootTag.Tags.Cast<NbtCompound>().First().Tags;




            /*
            var blocks = nbt00.RootTag.Tags.Cast<NbtCompound>().First()["Blocks"].ByteArrayValue;
            var meta = nbt00.RootTag.Tags.Cast<NbtCompound>().First()["Metadata"].ByteArrayValue;
            var storage = new BlockStorage64(8, 16 * 16 * 128);
            for (int i = 0; i < 16 * 16 * 128; i++)
            {
                storage[i] = (blocks[i], meta[i]);
            }
            */

            ;
            Console.WriteLine("Hello World!");
        }
    }
}
