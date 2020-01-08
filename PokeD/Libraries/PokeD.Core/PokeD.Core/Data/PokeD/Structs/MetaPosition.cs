using PokeD.Core.Extensions;

namespace PokeD.Core.Data.PokeD.Structs
{
    public struct MetaPosition
    {
        public long Meta { get; private set; }
        
        public Vector3 Position
        {
            get => new Vector3(Meta.BitsGet(0, 12) / 32.0f, Meta.BitsGet(24, 32) / 32.0f, Meta.BitsGet(12, 24) / 32.0f);
            set
            {
                Meta = Meta.BitsSet((long) (value.X * 32.0f), 0, 12);
                Meta = Meta.BitsSet((long) (value.Y * 32.0f), 24, 32);
                Meta = Meta.BitsSet((long) (value.Z * 32.0f), 12, 24);
            }
        }


        public MetaPosition(Vector3 position) : this() { Position = position; }
        public MetaPosition(long meta) { Meta = meta; }
    }
}