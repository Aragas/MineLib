using System;

namespace PokeD.Core.Data.SCON
{
    public class PlayerInfo
    {
        public string Name;
        public string IP;
        public ushort Ping;
        public Vector3 Position;
        public string LevelFile;
        public TimeSpan PlayTime;
    }
}