using Microsoft.EntityFrameworkCore;

using System;

namespace MineLib.Server.Heartbeat.Infrastructure.Data
{
    public class ClassicServersContext : DbContext
    {
        public DbSet<ClassicServer> Servers { get; set; } = default!;

        public ClassicServersContext(DbContextOptions<ClassicServersContext> options) : base(options) { }

        public void AddOrUpdate(ClassicServer entity)
        {
            var found = Servers.Find(entity.Hash);
            if (found == null)
            {
                entity.Added = DateTimeOffset.UtcNow;
                entity.LastUpdate = DateTimeOffset.UtcNow;
                Servers.Add(entity);
            }
            else
            {
                found.LastUpdate = DateTimeOffset.UtcNow;
                Servers.Update(found);
            }
        }
    }
}