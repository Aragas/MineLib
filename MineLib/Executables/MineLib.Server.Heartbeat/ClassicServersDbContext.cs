using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MineLib.Server.Heartbeat
{
    public static class ContextExtensions
    {
        public static void AddOrUpdate(this Microsoft.EntityFrameworkCore.DbContext ctx, ClassicServer entity)
        {
            var found = ctx.Find<ClassicServer>(entity.Hash);
            if (found == null)
            {
                entity.Added = DateTimeOffset.UtcNow;
                entity.LastUpdate = DateTimeOffset.UtcNow;
                ctx.Add(entity);
            }
            else
            {
                found.LastUpdate = DateTimeOffset.UtcNow;
                ctx.Update(found);
            }
            return;

            var entry = ctx.Entry(entity);
            switch (entry.State)
            {
                case EntityState.Detached:
                    entity.Added = DateTimeOffset.UtcNow;
                    entity.LastUpdate = DateTimeOffset.UtcNow;
                    ctx.Add(entity);
                    break;
                case EntityState.Modified:
                    entity.LastUpdate = DateTimeOffset.UtcNow;
                    ctx.Update(entity);
                    break;
                case EntityState.Added:
                    entity.Added = DateTimeOffset.UtcNow;
                    entity.LastUpdate = DateTimeOffset.UtcNow;
                    ctx.Add(entity);
                    break;
                case EntityState.Unchanged:
                    //item already in db no need to do anything  
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class ClassicServer
    {
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; } = default!;

        [Required, MaxLength(64)]
        public string Name { get; set; } = default!;

        [Required, MaxLength(45)]
        public string IP { get; set; } = default!;

        [Required]
        public ushort Port { get; set; } = default!;

        [Key]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "This field must be 32 characters")]
        public string Hash { get; set; } = default!;

        [Required, MaxLength(256)]
        public string Salt { get; set; } = default!;

        [Required]
        public int Players { get; set; } = default!;

        [Required]
        public int MaxPlayers { get; set; } = default!;

        [Required]
        public bool IsPublic { get; set; } = default!;

        public int? Version { get; set; } = default!;

        [MaxLength(256)]
        public string? Software { get; set; } = default!;

        public bool? IsSupportingWeb { get; set; } = default!;

        [Required]
        public DateTimeOffset Added { get; set; } = default!;

        [Required]
        public DateTimeOffset LastUpdate { get; set; } = default!;
    }
    public class ClassicServersDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<ClassicServer> Servers { get; set; } = default!;

        public ClassicServersDbContext(DbContextOptions<ClassicServersDbContext> options) : base(options) { }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassicServer>()
                .HasKey(m => m.Id);
            modelBuilder.Entity<ClassicServer>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ClassicServer>()
                .Property(m => m.Name)
                .HasMaxLength(64);

            modelBuilder.Entity<ClassicServer>()
                .Property(m => m.IP)
                .HasMaxLength(16);
        }
        */
    }
}