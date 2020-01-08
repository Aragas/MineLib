using Aragas.QServer.Core.IO;

using Microsoft.EntityFrameworkCore;

using MineLib.Core;
using MineLib.Core.Anvil;
using MineLib.Server.Core;
using MineLib.Server.WorldBus.Generator;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MineLib.Server.WorldBus
{
    public class WorldContext : DbContext
    {
        public sealed class PGSection
        {
            //public static implicit operator PGSection(Section section) => new PGSection(section);
            //public static implicit operator Section(PGSection section) => section.ToSection();

            public Location3D GetLocation()
            {
                ulong location = (ulong)Location - 1;
                return Location3D.FromLong(in location);
            }

            public Guid Id { get; set; } = default!;
            public long Location { get; set; } = default!;
            public byte[] SerializedSection { get; set; } = default!;

            public PGSection() { }
            public PGSection(Section section)
            {
                using var serializer = new CompressedProtobufSerializer();
                serializer.Write(section);

                Location = section.Location.GetDatabaseIndex();
                SerializedSection = serializer.GetData().ToArray();
            }

            public Section ToSection()
            {
                using var deserialiser = new CompressedProtobufDeserializer(SerializedSection);
                return deserialiser.Read<Section>();
            }
        }

        public DbSet<PGSection> Sections { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql(MineLibSingleton.PostgreSQLConnectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PGSection>()
                .HasKey(e => e.Id);
            modelBuilder.Entity<PGSection>()
                .HasIndex(e => e.Location)
                .IsUnique(true);
        }
    }


    public sealed class StandardWorldHandler : IWorldHandler
    {
        private IWorldGenerator Generator { get; } = new StandardGenerator();

        public StandardWorldHandler()
        {
            using var worldContext = new WorldContext();
            worldContext.Database.EnsureCreated();
            Generator.Initialize(null);
        }

        public Chunk GetChunk(Location2D location)
        {
            Chunk chunk;

            var sectionLocation = new List<long>();
            for (var i = 0; i < Chunk.SectionCount; i++)
                sectionLocation.Add(new Location3D(location.X, i, location.Z).GetDatabaseIndex());

            using var worldContext = new WorldContext();
            var result = Queryable.Where(worldContext.Sections, s => sectionLocation.Contains(s.Location))
                .AsEnumerable()
                .Select(s => s.ToSection())
                .OrderBy(s => s.Location.Y)
                .ToList();

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
            using var worldContext = new WorldContext();
            worldContext.Sections.AddRange(chunk.Sections.Select(s => new WorldContext.PGSection(s)));
            worldContext.SaveChanges();
        }

        public Section GetSection(Location3D chunkLocation)
        {
            var location = chunkLocation.GetDatabaseIndex();
            using var worldContext = new WorldContext();
            var section = worldContext.Sections.FirstOrDefault(s => s.Location == location)?.ToSection();

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
            using var worldContext = new WorldContext();
            worldContext.Sections.Add(new WorldContext.PGSection(section));
            worldContext.SaveChanges();
        }

        public ReadonlyBlock32 GetBlock(in Location3D blockWorldLocation) => GetSection(Chunk.GetSectionLocation(blockWorldLocation)).GetBlock(blockWorldLocation);
        public void SetBlock(in Location3D blockWorldLocation, in ReadonlyBlock32 block)
        {
            var section = GetSection(Chunk.GetSectionLocation(blockWorldLocation));
            section.SetBlock(Chunk.GetLocationInSection(blockWorldLocation), block);

            using var worldContext = new WorldContext();
            worldContext.Sections.Update(new WorldContext.PGSection(section));
            worldContext.SaveChanges();
        }
    }
}