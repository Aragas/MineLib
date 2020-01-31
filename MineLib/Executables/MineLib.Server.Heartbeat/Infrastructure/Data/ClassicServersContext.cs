using Microsoft.EntityFrameworkCore;

using System;

namespace MineLib.Server.Heartbeat.Infrastructure.Data
{
    public sealed class ClassicServersContext : DbContext
    {
        public DbSet<ClassicServer> Servers { get; set; } = default!;

        public ClassicServersContext(DbContextOptions<ClassicServersContext> options) : base(options) { }
    }
}