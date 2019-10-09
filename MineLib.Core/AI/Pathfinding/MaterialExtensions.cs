namespace MineLib.Core.AI.Pathfinding
{
    /// <summary>
    /// Defines extension methods for the Material enumeration
    /// </summary>
    public static class MaterialExtensions
    {
        /// <summary>
        /// Check if the player cannot pass through the specified material
        /// </summary>
        /// <param name="m">Material to test</param>
        /// <returns>True if the material is harmful</returns>
        public static bool IsSolid(this Material m)
        {
            switch (m)
            {
                case Material.Stone:
                case Material.Grass:
                case Material.Dirt:
                case Material.Cobblestone:
                case Material.Wood:
                case Material.Bedrock:
                case Material.Sand:
                case Material.Gravel:
                case Material.GoldOre:
                case Material.IronOre:
                case Material.CoalOre:
                case Material.Log:
                case Material.Leaves:
                case Material.Sponge:
                case Material.Glass:
                case Material.LapisOre:
                case Material.LapisBlock:
                case Material.Dispenser:
                case Material.Sandstone:
                case Material.NoteBlock:
                case Material.BedBlock:
                case Material.PistonStickyBase:
                case Material.PistonBase:
                case Material.PistonExtension:
                case Material.Wool:
                case Material.PistonMovingPiece:
                case Material.GoldBlock:
                case Material.IronBlock:
                case Material.DoubleStep:
                case Material.Step:
                case Material.Brick:
                case Material.Tnt:
                case Material.Bookshelf:
                case Material.MossyCobblestone:
                case Material.Obsidian:
                case Material.MobSpawner:
                case Material.WoodStairs:
                case Material.Chest:
                case Material.DiamondOre:
                case Material.DiamondBlock:
                case Material.Workbench:
                case Material.Soil:
                case Material.Furnace:
                case Material.BurningFurnace:
                case Material.SignPost:
                case Material.WoodenDoor:
                case Material.CobblestoneStairs:
                case Material.WallSign:
                case Material.StonePlate:
                case Material.IronDoorBlock:
                case Material.WoodPlate:
                case Material.RedstoneOre:
                case Material.GlowingRedstoneOre:
                case Material.Ice:
                case Material.SnowBlock:
                case Material.Cactus:
                case Material.Clay:
                case Material.Jukebox:
                case Material.Fence:
                case Material.Pumpkin:
                case Material.Netherrack:
                case Material.SoulSand:
                case Material.Glowstone:
                case Material.JackOLantern:
                case Material.CakeBlock:
                case Material.StainedGlass:
                case Material.TrapDoor:
                case Material.MonsterEggs:
                case Material.SmoothBrick:
                case Material.HugeMushroom1:
                case Material.HugeMushroom2:
                case Material.IronFence:
                case Material.ThinGlass:
                case Material.MelonBlock:
                case Material.FenceGate:
                case Material.BrickStairs:
                case Material.SmoothStairs:
                case Material.Mycel:
                case Material.NetherBrick:
                case Material.NetherFence:
                case Material.NetherBrickStairs:
                case Material.EnchantmentTable:
                case Material.BrewingStand:
                case Material.Cauldron:
                case Material.EnderPortalFrame:
                case Material.EnderStone:
                case Material.DragonEgg:
                case Material.RedstoneLampOff:
                case Material.RedstoneLampOn:
                case Material.WoodDoubleStep:
                case Material.WoodStep:
                case Material.SandstoneStairs:
                case Material.EmeraldOre:
                case Material.EnderChest:
                case Material.EmeraldBlock:
                case Material.SpruceWoodStairs:
                case Material.BirchWoodStairs:
                case Material.JungleWoodStairs:
                case Material.Command:
                case Material.Beacon:
                case Material.CobbleWall:
                case Material.Anvil:
                case Material.TrappedChest:
                case Material.GoldPlate:
                case Material.IronPlate:
                case Material.DaylightDetector:
                case Material.RedstoneBlock:
                case Material.QuartzOre:
                case Material.Hopper:
                case Material.QuartzBlock:
                case Material.QuartzStairs:
                case Material.Dropper:
                case Material.StainedClay:
                case Material.HayBlock:
                case Material.HardClay:
                case Material.CoalBlock:
                case Material.StainedGlassPane:
                case Material.Leaves2:
                case Material.Log2:
                case Material.AcaciaStairs:
                case Material.DarkOakStairs:
                case Material.PackedIce:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check if contact with the provided material can harm players
        /// </summary>
        /// <param name="m">Material to test</param>
        /// <returns>True if the material is harmful</returns>
        public static bool CanHarmPlayers(this Material m)
        {
            switch (m)
            {
                case Material.Fire:
                case Material.Cactus:
                case Material.Lava:
                case Material.StationaryLava:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Check if the provided material is a liquid a player can swim into
        /// </summary>
        /// <param name="m">Material to test</param>
        /// <returns>True if the material is a liquid</returns>
        public static bool IsLiquid(this Material m)
        {
            switch (m)
            {
                case Material.Water:
                case Material.StationaryWater:
                case Material.Lava:
                case Material.StationaryLava:
                    return true;
                default:
                    return false;
            }
        }
    }
}