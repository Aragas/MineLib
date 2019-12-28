using System;

using MineLib.Core.Data;
using MineLib.Core.Extensions;

namespace ProtocolModern.Data
{
    public struct ChunkColumnMetadata : IEquatable<ChunkColumnMetadata>
    {
        public Coordinates2D Coordinates;
        public ushort PrimaryBitMap;
        public ushort AddBitMap;

        // -- Debugging
        public bool[] PrimaryBitMapConverted => Helper.ConvertFromUShort(PrimaryBitMap);
        // -- Debugging
        // -- Debugging
        public bool[] AddBitMapConverted => Helper.ConvertFromUShort(AddBitMap);
        // -- Debugging

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            return Equals((ChunkColumnMetadata) obj);
        }

        public bool Equals(ChunkColumnMetadata other)
        {
            return Coordinates == other.Coordinates && PrimaryBitMap == other.PrimaryBitMap;
        }

        public override int GetHashCode()
        {
            return Coordinates.GetHashCode() ^ PrimaryBitMap.GetHashCode();
        }
    }
}
