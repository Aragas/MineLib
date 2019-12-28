using MineLib.Core.Anvil;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MineLib.Core.AI.Pathfinding
{
    //https://github.com/ORelio/Minecraft-Console-Client/blob/a344ac4101af99d3dd9a4e48c3a12a6c8e989c28/MinecraftClient/Mapping/Movement.cs
    /// <summary>
    /// Allows moving through a Minecraft world
    /// </summary>
    public static class Movement
    {
        /* ========= PATHFINDING METHODS ========= */

        /// <summary>
        /// Handle movements due to gravity
        /// </summary>
        /// <param name="world">World the player is currently located in</param>
        /// <param name="location">Vector3 the player is currently at</param>
        /// <param name="motionY">Current vertical motion speed</param>
        /// <returns>Updated location after applying gravity</returns>
        public static Vector3 HandleGravity(IWorld world, in Vector3 location, ref float motionY)
        {
            Vector3 updatedLocation = location;
            var onFoots = new Vector3(location.X, MathF.Floor(location.Y), location.Z);
            var belowFoots = Move(in location, Direction.Down);
            if (location.Y > MathF.Truncate(location.Y) + 0.0001)
            {
                belowFoots = location;
                belowFoots.Y = MathF.Truncate(location.Y);
            }
            if (!IsOnGround(world, in location) && !IsSwimming(world, in location))
            {
                while (!IsOnGround(world, in belowFoots) && belowFoots.Y >= 1)
                    belowFoots = Move(in belowFoots, Direction.Down);
                updatedLocation = Move2Steps(in location, in belowFoots, ref motionY, true).Dequeue();
            }
            else if (!world.GetBlock(onFoots).GetMaterial().IsSolid())
                updatedLocation = Move2Steps(in location, in onFoots, ref motionY, true).Dequeue();
            return updatedLocation;
        }

        /// <summary>
        /// Return a list of possible moves for the player
        /// </summary>
        /// <param name="world">World the player is currently located in</param>
        /// <param name="location">Vector3 the player is currently at</param>
        /// <param name="allowUnsafe">Allow possible but unsafe locations</param>
        /// <returns>A list of new locations the player can move to</returns>
        public static IEnumerable<Vector3> GetAvailableMoves(IWorld world, in Vector3 location, bool allowUnsafe = false)
        {
            var availableMoves = new List<Vector3>();
            if (IsOnGround(world, in location) || IsSwimming(world, in location))
            {
                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                    if (CanMove(world, in location, dir) && (allowUnsafe || IsSafe(world, Move(in location, dir))))
                        availableMoves.Add(Move(in location, dir));
            }
            else
            {
                foreach (Direction dir in new[] { Direction.East, Direction.West, Direction.North, Direction.South })
                    if (CanMove(world, in location, dir) && IsOnGround(world, Move(in location, dir)) && (allowUnsafe || IsSafe(world, Move(in location, dir))))
                        availableMoves.Add(Move(in location, dir));
                availableMoves.Add(Move(in location, Direction.Down));
            }
            return availableMoves;
        }

        /// <summary>
        /// Decompose a single move from a block to another into several steps
        /// </summary>
        /// <remarks>
        /// Allows moving by little steps instead or directly moving between blocks,
        /// which would be rejected by anti-cheat plugins anyway.
        /// </remarks>
        /// <param name="start">Start location</param>
        /// <param name="goal">Destination location</param>
        /// <param name="motionY">Current vertical motion speed</param>
        /// <param name="falling">Specify if performing falling steps</param>
        /// <param name="stepsByBlock">Amount of steps by block</param>
        /// <returns>A list of locations corresponding to the requested steps</returns>
        public static Queue<Vector3> Move2Steps(in Vector3 start, in Vector3 goal, ref float motionY, bool falling = false, int stepsByBlock = 8)
        {
            if (stepsByBlock <= 0)
                stepsByBlock = 1;

            if (falling)
            {
                //Use MC-Like falling algorithm
                var Y = start.Y;
                var fallSteps = new Queue<Vector3>();
                fallSteps.Enqueue(start);
                var motionPrev = motionY;
                motionY -= 0.08F;
                motionY *= 0.9800000190734863F;
                Y += motionY;
                if (Y < goal.Y)
                    return new Queue<Vector3>(new[] { goal });
                else return new Queue<Vector3>(new[] { new Vector3(start.X, Y, start.Z) });
            }
            else
            {
                //Regular MCC moving algorithm
                motionY = 0; //Reset motion speed
                var totalStepsDouble = start.Distance(in goal) * stepsByBlock;
                var totalSteps = (int) MathF.Ceiling(totalStepsDouble);
                var step = (goal - start) / totalSteps;

                if (totalStepsDouble >= 1)
                {
                    var movementSteps = new Queue<Vector3>();
                    for (int i = 1; i <= totalSteps; i++)
                        movementSteps.Enqueue(start + (step * i));
                    return movementSteps;
                }
                else return new Queue<Vector3>(new[] { goal });
            }
        }

        /// <summary>
        /// Calculate a path from the start location to the destination location
        /// </summary>
        /// <remarks>
        /// Based on the A* pathfinding algorithm described on Wikipedia
        /// </remarks>
        /// <see href="https://en.wikipedia.org/wiki/A*_search_algorithm#Pseudocode"/>
        /// <param name="start">Start location</param>
        /// <param name="goal">Destination location</param>
        /// <param name="allowUnsafe">Allow possible but unsafe locations</param>
        /// <returns>A list of locations, or null if calculation failed</returns>
        public static Queue<Vector3> CalculatePath(IWorld world, in Vector3 start, in Vector3 goal, bool allowUnsafe = false)
        {
            Queue<Vector3> result = null;

            var closedSet = new HashSet<Vector3>(); // The set of locations already evaluated.
            var openSet = new HashSet<Vector3>(new[] { start });  // The set of tentative nodes to be evaluated, initially containing the start node
            var came_From = new Dictionary<Vector3, Vector3>(); // The map of navigated nodes.

            var g_score = new Dictionary<Vector3, int>(); //:= map with default value of Infinity
            g_score[start] = 0; // Cost from start along best known path.
                                // Estimated total cost from start to goal through y.
            var f_score = new Dictionary<Vector3, int>(); //:= map with default value of Infinity
            f_score[start] = (int) start.DistanceSquared(in goal); //heuristic_cost_estimate(start, goal)

            while (openSet.Count > 0)
            {
                var current = //the node in OpenSet having the lowest f_score[] value
                    openSet.Select(location => f_score.ContainsKey(location)
                    ? new KeyValuePair<Vector3, int>(location, f_score[location])
                    : new KeyValuePair<Vector3, int>(location, int.MaxValue))
                    .OrderBy(pair => pair.Value).First().Key;
                if (current == goal)
                { //reconstruct_path(Came_From, goal)
                    var total_path = new List<Vector3>(new[] { current });
                    while (came_From.ContainsKey(current))
                    {
                        current = came_From[current];
                        total_path.Add(current);
                    }
                    total_path.Reverse();
                    result = new Queue<Vector3>(total_path);
                }
                openSet.Remove(current);
                closedSet.Add(current);
                foreach (var neighbor in GetAvailableMoves(world, in current, allowUnsafe))
                {
                    if (closedSet.Contains(neighbor))
                        continue;       // Ignore the neighbor which is already evaluated.
                    var tentative_g_score = g_score[current] + (int) current.DistanceSquared(in neighbor); //dist_between(current,neighbor) // length of this path.
                    if (!openSet.Contains(neighbor))    // Discover a new node
                        openSet.Add(neighbor);
                    else if (tentative_g_score >= g_score[neighbor])
                        continue;       // This is not a better path.

                    // This path is the best until now. Record it!
                    came_From[neighbor] = current;
                    g_score[neighbor] = tentative_g_score;
                    f_score[neighbor] = g_score[neighbor] + (int) neighbor.DistanceSquared(in goal); //heuristic_cost_estimate(neighbor, goal)
                }
            }

            return result;
        }

        /* ========= LOCATION PROPERTIES ========= */

        /// <summary>
        /// Check if the specified location is on the ground
        /// </summary>
        /// <param name="world">World for performing check</param>
        /// <param name="location">Vector3 to check</param>
        /// <returns>True if the specified location is on the ground</returns>
        private static bool IsOnGround(IWorld world, in Vector3 location) => world.GetBlock(Move(in location, Direction.Down)).GetMaterial().IsSolid() && (location.Y <= MathF.Truncate(location.Y) + 0.0001);

        /// <summary>
        /// Check if the specified location implies swimming
        /// </summary>
        /// <param name="world">World for performing check</param>
        /// <param name="location">Vector3 to check</param>
        /// <returns>True if the specified location implies swimming</returns>
        private static bool IsSwimming(IWorld world, in Vector3 location) => world.GetBlock(location).GetMaterial().IsLiquid();

        /// <summary>
        /// Check if the specified location is safe
        /// </summary>
        /// <param name="world">World for performing check</param>
        /// <param name="location">Vector3 to check</param>
        /// <returns>True if the destination location won't directly harm the player</returns>
        private static bool IsSafe(IWorld world, in Vector3 location)
        {
            return
                //No block that can harm the player
                   !world.GetBlock(location).GetMaterial().CanHarmPlayers()
                && !world.GetBlock(Move(in location, Direction.Up)).GetMaterial().CanHarmPlayers()
                && !world.GetBlock(Move(in location, Direction.Down)).GetMaterial().CanHarmPlayers()

                //No fall from a too high place
                && (world.GetBlock(Move(in location, Direction.Down)).GetMaterial().IsSolid()
                     || world.GetBlock(Move(in location, Direction.Down, 2)).GetMaterial().IsSolid()
                     || world.GetBlock(Move(in location, Direction.Down, 3)).GetMaterial().IsSolid())

                //Not an underwater location
                && !world.GetBlock(Move(in location, Direction.Up)).GetMaterial().IsLiquid();
        }

        /* ========= SIMPLE MOVEMENTS ========= */

        /// <summary>
        /// Check if the player can move in the specified direction
        /// </summary>
        /// <param name="world">World the player is currently located in</param>
        /// <param name="location">Vector3 the player is currently at</param>
        /// <param name="direction">Direction the player is moving to</param>
        /// <returns>True if the player can move in the specified direction</returns>
        private static bool CanMove(IWorld world, in Vector3 location, Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return !IsOnGround(world, in location);
                case Direction.Up:
                    return (IsOnGround(world, in location) || IsSwimming(world, in location))
                        && !world.GetBlock(Move(Move(in location, Direction.Up), Direction.Up)).GetMaterial().IsSolid();
                case Direction.East:
                case Direction.West:
                case Direction.South:
                case Direction.North:
                    return !world.GetBlock(Move(in location, direction)).GetMaterial().IsSolid()
                        && !world.GetBlock(Move(Move(in location, direction), Direction.Up)).GetMaterial().IsSolid();
                default:
                    throw new ArgumentException("Unknown direction", nameof(direction));
            }
        }

        /// <summary>
        /// Get an updated location for moving in the specified direction
        /// </summary>
        /// <param name="location">Current location</param>
        /// <param name="direction">Direction to move to</param>
        /// <param name="length">Distance, in blocks</param>
        /// <returns>Updated location</returns>
        private static Vector3 Move(in Vector3 location, Direction direction, int length = 1) => location + (Move(direction) * length);

        /// <summary>
        /// Get a location delta for moving in the specified direction
        /// </summary>
        /// <param name="direction">Direction to move to</param>
        /// <returns>A location delta for moving in that direction</returns>
        private static Vector3 Move(Direction direction)
        {
            return direction switch
            {
                Direction.Down => new Vector3(0, -1, 0),
                Direction.Up => new Vector3(0, 1, 0),
                Direction.East => new Vector3(1, 0, 0),
                Direction.West => new Vector3(-1, 0, 0),
                Direction.South => new Vector3(0, 0, 1),
                Direction.North => new Vector3(0, 0, -1),
                _ => throw new ArgumentException("Unknown direction", nameof(direction))
            };
        }
    }
}