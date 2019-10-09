using System;
using System.Numerics;

namespace MineLib.Core.AI.Pathfinding
{
    public static class Vector3Extensions
    {
        public static float DistanceSquared(this in Vector3 location1, in Vector3 location2)
        {
            return ((location1.X - location2.X) * (location1.X - location2.X))
                 + ((location1.Y - location2.Y) * (location1.Y - location2.Y))
                 + ((location1.Z - location2.Z) * (location1.Z - location2.Z));
        }

        /// <summary>
        /// Get exact distance to the specified location
        /// </summary>
        /// <param name="location2">Other location for computing distance</param>
        /// <returns>Distance to the specified location, with square root so lower performances</returns>
        public static float Distance(this in Vector3 location1, in Vector3 location2) => MathF.Sqrt(location1.DistanceSquared(location2));
    }
}