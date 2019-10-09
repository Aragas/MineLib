
using MineLib.Core;

namespace MineLib.Server.WorldBus
{
    public static class LocationExtensions
    {
        /// <summary>
        /// (0, 0, 0) will give value 0, which our database will ignore.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static long GetDatabaseIndex(this in Location3D location) => (long) location.ToLong() + 1;
    }
}