using System;
using System.Numerics;

namespace MineLib.Core
{
    public interface IEntity
    {
        Vector3 Position { get; set; }
        Look Look { get; set; }
    }

    public interface IPlayer : IEntity
    {
        string Username { get; set; }
        Guid? Uuid { get; set; }
        /*
        Gamemode Gamemode { get; set; } //The player's gamemode
        bool IsSpawned { get; set; } //Is the player spawned?
        bool Digging { get; set; } // Is the player digging?
        bool CanFly { get; set; } //Can the player fly?

        string Locale { get; set; }
        byte ViewDistance { get; set; }
        int ChatFlags { get; set; }
        bool ChatColours { get; set; }
        byte SkinParts { get; set; }
        int MainHand { get; set; }
        bool ForceChunkReload { get; set; }
        EntityAction LastEntityAction { get; set; }
        bool IsOperator { get; internal set; }
        bool Loaded { get; set; }
        bool IsCrouching { get; set; }
        */
    }
    public class Entity : IEntity
    {
        public int ID { get; set; }

        public Vector3 Position { get; set; }
        public Look Look { get; set; }
    }
    public class Player : Entity, IPlayer
    {
        public string Username { get; set; }

        public Guid? Uuid { get; set; }

        public bool IsOnline { get; set; }
        public string? BanInfo { get; set; }
    }
}