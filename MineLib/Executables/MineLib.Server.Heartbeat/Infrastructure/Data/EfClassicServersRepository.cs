using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;

namespace MineLib.Server.Heartbeat.Infrastructure.Data
{
    public class EfClassicServersRepository : IClassicServersRepository
    {
        private readonly ClassicServersContext _dbContext;

        public EfClassicServersRepository(ClassicServersContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ClassicServer? GetByHash(string hash)
        {
            return _dbContext.Set<ClassicServer>().SingleOrDefault(e => e.Hash == hash);
        }

        public List<ClassicServer> List()
        {
            return _dbContext.Set<ClassicServer>().ToList();
        }

        public ClassicServer Add(ClassicServer entity)
        {
            _dbContext.Set<ClassicServer>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public void Delete(ClassicServer entity)
        {
            _dbContext.Set<ClassicServer>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update(ClassicServer entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
