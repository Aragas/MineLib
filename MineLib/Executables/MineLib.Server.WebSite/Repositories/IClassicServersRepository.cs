using MineLib.Server.WebSite.Models;

using System.Collections.Generic;

namespace MineLib.Server.WebSite.Repositories
{
    public interface IClassicServersRepository
    {
        ClassicServer? GetByHash(string hash);
        List<ClassicServer> List();
        ClassicServer Add(ClassicServer entity);
        void Update(ClassicServer entity);
        void Delete(ClassicServer entity);
    }
}