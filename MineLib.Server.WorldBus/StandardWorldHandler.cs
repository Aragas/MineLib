using LiteDB;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Server.WorldBus.Generator;

using System.Collections.Generic;
using System.Linq;

namespace MineLib.Server.WorldBus
{
    public sealed class StandardWorldHandler : IWorldHandler
    {
        private const string DatabaseName = "World.litedb";

        private IWorldGenerator Generator { get; } = new StandardGenerator();

        public StandardWorldHandler()
        {
            Generator.Initialize(null);
        }

        public Chunk GetChunk(Location2D location)
        {
            Chunk chunk;

            var sectionLocation = new List<long>();
            for (var i = 0; i < Chunk.SectionCount; i++)
                sectionLocation.Add(new Location3D(location.X, i, location.Z).GetDatabaseIndex());

            using var db = new LiteDatabase(DatabaseName);
            var sections = db.GetCollection<DBSection>("sections");
            var result = sections.Find(s => sectionLocation.Contains(s.Location)).Select(s => s.ToSection()).OrderBy(s => s.Location.Y).ToList();

            if (result.Count == 0)
            {
                chunk = Generator.GenerateChunk(location);
                SetChunk(in chunk);
                return chunk;
            }

            chunk = new Chunk(location);
            for (var i = 0; i < Chunk.SectionCount; i++)
                chunk.Sections[i] = result[i];
            return chunk;
        }
        public void SetChunk(in Chunk chunk)
        {
            using var db = new LiteDatabase(DatabaseName);
            var sections = db.GetCollection<DBSection>("sections");
            sections.InsertBulk(chunk.Sections.OrderBy(s => s.Location.Y).Select(s => new DBSection(s)));
            sections.EnsureIndex(x => x.Location);
        }

        public Section GetSection(Location3D chunkLocation)
        {
            Section? section;
            using var db = new LiteDatabase(DatabaseName);
            var sections = db.GetCollection<DBSection>("sections");
            section = sections.FindById(new BsonValue(chunkLocation.GetDatabaseIndex()))?.ToSection();

            if (section == null)
            {
                var chunk = Generator.GenerateChunk(new Location2D(chunkLocation.X, chunkLocation.Y));
                SetChunk(chunk);
                return chunk.Sections[chunkLocation.Y];
            }

            return section.Value;
        }
        public void SetSection(in Section section)
        {
            using var db = new LiteDatabase(DatabaseName);
            var sections = db.GetCollection<DBSection>("sections");
            sections.Insert(new DBSection(section));
            sections.EnsureIndex(x => x.Location);
        }

        public ReadonlyBlock32 GetBlock(in Location3D blockWorldLocation) => GetSection(Chunk.GetSectionLocation(blockWorldLocation)).GetBlock(blockWorldLocation);
        public void SetBlock(in Location3D blockWorldLocation, in ReadonlyBlock32 block)
        {
            var section = GetSection(Chunk.GetSectionLocation(blockWorldLocation));
            section.SetBlock(Chunk.GetLocationInSection(blockWorldLocation), block);

            using var db = new LiteDatabase(DatabaseName);
            var sections = db.GetCollection<DBSection>("sections");
            sections.Update(new DBSection(section));
        }
    }
}