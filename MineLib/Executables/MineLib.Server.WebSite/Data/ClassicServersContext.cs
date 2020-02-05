using Microsoft.EntityFrameworkCore;

using MineLib.Server.WebSite.Models;

namespace MineLib.Server.WebSite.Data
{
    public sealed class ClassicServersContext : DbContext
    {
        public DbSet<ClassicServer> Servers { get; set; } = default!;

        public ClassicServersContext(DbContextOptions<ClassicServersContext> options) : base(options) { }
    }
}