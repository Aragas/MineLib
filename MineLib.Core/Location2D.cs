using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MineLib.Core
{
    /// <summary>
    /// Represents the location of an object in 2D space (int).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Location2D : IEquatable<Location2D>
    {
        public readonly int X;
        public readonly int Z;

        public Location2D(int value) { X = Z = value; }
        public Location2D(int x, int z) { X = x; Z = z; }
        public Location2D(in Location2D location) { X = location.X; Z = location.Z; }


        /// <summary>
        /// Converts this Coordinates2D to a string.
        /// </summary>
        public override string ToString() => $"X: {X}, Z: {Z}";

        #region Math

        private static int Square(int num) => num * num;

        /// <summary>
        /// Calculates the distance between two Coordinates2D objects.
        /// </summary>
        public double DistanceTo(in Location2D other) => Math.Sqrt(Square(other.X - X) + Square(other.Z - Z));

        /// <summary>
        /// Finds the distance of this Coordinates2D from Coordinates2D.Zero
        /// </summary>
        public double Distance() => DistanceTo(Zero);

        public static Location2D Min(in Location2D a, in Location2D b) => new Location2D(Math.Min(a.X, b.X), Math.Min(a.Z, b.Z));
        public static Location2D Max(in Location2D a, in Location2D b) => new Location2D(Math.Max(a.X, b.X), Math.Max(a.Z, b.Z));

        #endregion

        #region Operators

        public static Location2D operator -(in Location2D a) => new Location2D(-a.X, -a.Z);
        public static Location2D operator ++(in Location2D a) => new Location2D(a.X, a.Z) + One;
        public static Location2D operator --(in Location2D a) => new Location2D(a.X, a.Z) - One;

        public static bool operator ==(in Location2D a, in Location2D b) => a.X == b.X && a.Z == b.Z;
        public static bool operator !=(in Location2D a, in Location2D b) => !(a == b);

        public static Location2D operator +(in Location2D a, in Location2D b) => new Location2D(a.X + b.X, a.Z + b.Z);
        public static Location2D operator -(in Location2D a, in Location2D b) => new Location2D(a.X - b.X, a.Z - b.Z);
        public static Location2D operator *(in Location2D a, in Location2D b) => new Location2D(a.X * b.X, a.Z * b.Z);
        public static Location2D operator /(in Location2D a, in Location2D b) => new Location2D(a.X / b.X, a.Z / b.Z);
        public static Location2D operator %(in Location2D a, in Location2D b) => new Location2D(a.X % b.X, a.Z % b.Z);

        public static Location2D operator +(in Location2D a, int b) => new Location2D(a.X + b, a.Z + b);
        public static Location2D operator -(in Location2D a, int b) => new Location2D(a.X - b, a.Z - b);
        public static Location2D operator *(in Location2D a, int b) => new Location2D(a.X * b, a.Z * b);
        public static Location2D operator /(in Location2D a, int b) => new Location2D(a.X / b, a.Z / b);
        public static Location2D operator %(in Location2D a, int b) => new Location2D(a.X % b, a.Z % b);

        public static Location2D operator +(int a, in Location2D b) => new Location2D(a + b.X, a + b.Z);
        public static Location2D operator -(int a, in Location2D b) => new Location2D(a - b.X, a - b.Z);
        public static Location2D operator *(int a, in Location2D b) => new Location2D(a * b.X, a * b.Z);
        public static Location2D operator /(int a, in Location2D b) => new Location2D(a / b.X, a / b.Z);
        public static Location2D operator %(int a, in Location2D b) => new Location2D(a % b.X, a % b.Z);

        #endregion

        #region Constants

        public static readonly Location2D Zero = new Location2D(0);
        public static readonly Location2D One = new Location2D(1);

        public static readonly Location2D Forward = new Location2D(0, 1);
        public static readonly Location2D Backward = new Location2D(0, -1);
        public static readonly Location2D Left = new Location2D(-1, 0);
        public static readonly Location2D Right = new Location2D(1, 0);

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Location2D location && Equals(location);
        public bool Equals(Location2D other) => other.X.Equals(X) && other.Z.Equals(Z);

        public override int GetHashCode() => HashCode.Combine(X, Z);
    }
}