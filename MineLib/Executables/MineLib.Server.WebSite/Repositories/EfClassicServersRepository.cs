using Microsoft.EntityFrameworkCore;

using MineLib.Server.WebSite.Data;
using MineLib.Server.WebSite.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MineLib.Server.WebSite.Repositories
{
    public sealed class EfClassicServersRepository : IClassicServersRepository
    {
        private readonly ClassicServersContext _dbContext;

        public EfClassicServersRepository(ClassicServersContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public ClassicServer? GetByHash(string hash)
        {
            return _dbContext.Database.CanConnect() ? _dbContext.Set<ClassicServer>().SingleOrDefault(e => e.Hash == hash) : null;
        }

        public List<ClassicServer> List()
        {
            return _dbContext.Database.CanConnect() ? _dbContext.Set<ClassicServer>().ToList() : new List<ClassicServer>();
        }

        public ClassicServer Add(ClassicServer entity)
        {
            if (_dbContext.Database.CanConnect())
            {
                _dbContext.Set<ClassicServer>().Add(entity);
                _dbContext.SaveChanges();
            }

            return entity;
        }

        public void Delete(ClassicServer entity)
        {
            if (_dbContext.Database.CanConnect())
            {
                _dbContext.Set<ClassicServer>().Remove(entity);
                _dbContext.SaveChanges();
            }
        }

        public void Update(ClassicServer entity)
        {
            if (_dbContext.Database.CanConnect())
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }
    }
}