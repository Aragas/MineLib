using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MineLib.Core
{
    /// <summary>
    /// Represents the location of an object in 3D space (int).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Location3D : IEquatable<Location3D>
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public Location3D(int value) { X = Y = Z = value; }
        public Location3D(int x, int y, int z) { X = x; Y = y; Z = z; }
        public Location3D(in Location3D location) { X = location.X; Y = location.Y; Z = location.Z; }

        public static Location3D FromLong(in ulong value) => new Location3D((int) (value >> 38), (int) (value & 0b11111111_1111), (int) (value << 26 >> 38));
        public ulong ToLong() => (((ulong) X & 0b11111111_11111111_11111111_11) << 38) | (((ulong) Z & 0b11111111_11111111_11111111_11) << 12) | ((ulong) Y & 0b11111111_1111);
        /*
        public static Location3D FromLong(in ulong value) => new Location3D((int) (value >> 38), (int) (value >> 26) & 0xFFF, (int) value << 38 >> 38);
        public ulong ToLong() => (((ulong) X & 0b1111111_11111111_1111111_11) << 38) | (((ulong) Y & 0b11111111_1111) << 26) | ((ulong) Z & 0b11111111_11111111_11111111_11);
        */

        /// <summary>
        /// Converts this Location to a string.
        /// </summary>
        public override string ToString() => $"X: {X}, Y: {Y}, Z: {Z}";

        #region Math

        /// <summary>
        /// Calculates the distance between two Location objects.
        /// </summary>
        public double DistanceTo(in Location3D other) => Math.Sqrt(Square(other.X - X) + Square(other.Y - Y) + Square(other.Z - Z));

        /// <summary>
        /// Calculates the square of a num.
        /// </summary>
        private static int Square(int num) => num * num;

        /// <summary>
        /// Finds the distance of this Location from Location.Zero
        /// </summary>
        public double Distance() => DistanceTo(Zero);

        public static Location3D Min(in Location3D a, in Location3D b) => new Location3D(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        public static Location3D Max(in Location3D a, in Location3D b) => new Location3D(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));

        #endregion

        #region Operators

        public static Location3D operator  -(in Location3D a) => new Location3D(-a.X, -a.Y, -a.Z);
        public static Location3D operator ++(in Location3D a) => new Location3D(a.X, a.Y, a.Z) + One;
        public static Location3D operator --(in Location3D a) => new Location3D(a.X, a.Y, a.Z) - One;

        public static bool operator ==(in Location3D a, in Location3D b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        public static bool operator !=(in Location3D a, in Location3D b) => !(a == b);

        public static Location3D operator +(in Location3D a, in Location3D b) => new Location3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Location3D operator -(in Location3D a, in Location3D b) => new Location3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Location3D operator *(in Location3D a, in Location3D b) => new Location3D(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        public static Location3D operator /(in Location3D a, in Location3D b) => new Location3D(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        public static Location3D operator %(in Location3D a, in Location3D b) => new Location3D(a.X % b.X, a.Y % b.Y, a.Z % b.Z);

        public static Location3D operator +(in Location3D a, int b) => new Location3D(a.X + b, a.Y + b, a.Z + b);
        public static Location3D operator -(in Location3D a, int b) => new Location3D(a.X - b, a.Y - b, a.Z - b);
        public static Location3D operator *(in Location3D a, int b) => new Location3D(a.X * b, a.Y * b, a.Z * b);
        public static Location3D operator /(in Location3D a, int b) => new Location3D(a.X / b, a.Y / b, a.Z / b);
        public static Location3D operator %(in Location3D a, int b) => new Location3D(a.X % b, a.Y % b, a.Z % b);

        public static Location3D operator +(int a, in Location3D b) => new Location3D(a + b.X, a + b.Y, a + b.Z);
        public static Location3D operator -(int a, in Location3D b) => new Location3D(a - b.X, a - b.Y, a - b.Z);
        public static Location3D operator *(int a, in Location3D b) => new Location3D(a * b.X, a * b.Y, a * b.Z);
        public static Location3D operator /(int a, in Location3D b) => new Location3D(a / b.X, a / b.Y, a / b.Z);
        public static Location3D operator %(int a, in Location3D b) => new Location3D(a % b.X, a % b.Y, a % b.Z);

        public static implicit operator Location3D(in Vector3 a) => new Location3D((int) a.X, (int) a.Y, (int) a.Z);
        public static implicit operator Vector3(in Location3D a) => new Vector3(a.X, a.Y, a.Z);

        #endregion

        #region Constants

        public static readonly Location3D Zero = new Location3D(0);
        public static readonly Location3D One = new Location3D(1);

        public static readonly Location3D Up = new Location3D(0, 1, 0);
        public static readonly Location3D Down = new Location3D(0, -1, 0);
        public static readonly Location3D Left = new Location3D(-1, 0, 0);
        public static readonly Location3D Right = new Location3D(1, 0, 0);
        public static readonly Location3D Backwards = new Location3D(0, 0, -1);
        public static readonly Location3D Forwards = new Location3D(0, 0, 1);

        public static readonly Location3D East = new Location3D(1, 0, 0);
        public static readonly Location3D West = new Location3D(-1, 0, 0);
        public static readonly Location3D North = new Location3D(0, 0, -1);
        public static readonly Location3D South = new Location3D(0, 0, 1);

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Location3D location && Equals(location);
        public bool Equals(Location3D other) => other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);
    }
}