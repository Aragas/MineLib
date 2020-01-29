using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System;
using System.ComponentModel.DataAnnotations;

namespace MineLib.Server.Heartbeat
{
    public static class ContextExtensions
    {
        public static void AddOrUpdate(this DbContext ctx, object entity)
        {
            var entry = ctx.Entry(entity);
            switch (entry.State)
            {
                case EntityState.Detached:
                    ctx.Add(entity);
                    break;
                case EntityState.Modified:
                    ctx.Update(entity);
                    break;
                case EntityState.Added:
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
        [Key]
        public int Id { get; set; } = default!;

        [Required, MaxLength(64)]
        public string Name { get; set; } = default!;
        [Required, MaxLength(16)]
        public string IP { get; set; } = default!;
        [Required]
        public ushort Port { get; set; } = default!;
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
        public DateTime LastUpdate { get; set; } = default!;
    }
    public class ClassicServersDbContext : DbContext
    {
        public DbSet<ClassicServer> Sections { get; set; } = default!;

        private readonly IConfiguration _configuration;

        public ClassicServersDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql(_configuration["PostgreSQLConnectionString"]);
    }
}