using Aragas.QServer.Core;

namespace MineLib.Server.Core
{
    public class MineLibSingleton : BaseSingleton
    {
        public static string PostgreSQLConnectionString { get; } = "Host=aragas.db;Port=5432;Database=minelib;Username=minelib;Password=minelib";
    }
}