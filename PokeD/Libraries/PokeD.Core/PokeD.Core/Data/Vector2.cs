using System.Runtime.InteropServices;

using Math = System.MathF;

namespace PokeD.Core.Data
{
    // If the size of a readonly struct is bigger than IntPtr.Size you should pass it as an in-parameter for performance reasons.
    // Maybe then do not use in-parameter here?

    /// <summary>
    /// Represents the location of an object in 2D space (float).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Vector2
    {
        public readonly float X;
        public readonly float Y;


        public Vector2(float value) { X = Y = value; }
        public Vector2(float x, float y) { X = x; Y = y; }
        public Vector2(double x, double y) { X = (float) x; Y = (float) y; }
        public Vector2(Vector2 v) { X = v.X; Y = v.Y; }


        /// <summary>
        /// Converts this Vector2 to a string.
        /// </summary>
        public override string ToString() => $"X: {X}, Y: {Y}";

        #region Math

        public static Vector2 Floor(in Vector2 value) => new Vector2(Math.Floor(value.X), Math.Floor(value.Y));
        public Vector2 Floor() => Floor(in this);

        public static Vector2 Ceiling(in Vector2 value) => new Vector2(Math.Ceiling(value.X), Math.Ceiling(value.Y));
        public Vector2 Ceiling() => Ceiling(in this);

        private static float Square(float num) => num * num;
        public static float DistanceTo(in Vector2 a, in Vector2 b) => a.DistanceTo(in b);
        public float DistanceTo(in Vector2 other) => (float) Math.Sqrt(Square(other.X - X) + Square(other.Y - Y));

        /// <summary>
        /// Finds the distance of this vector from Vector2.Zero
        /// </summary>
        public double Distance() => DistanceTo(in Zero);

        public static Vector2 Min(in Vector2 a, in Vector2 b) => new Vector2(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        public Vector2 Min(Vector2 other) => new Vector2(Math.Min(X, other.X), Math.Min(Y, other.Y));

        public static Vector2 Max(in Vector2 a, in Vector2 b) => new Vector2(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        public Vector2 Max(in Vector2 other) => new Vector2(Math.Max(X, other.X), Math.Max(Y, other.Y));

        public static Vector2 Delta(in Vector2 a, in Vector2 b) => a - b;
        public Vector2 Delta(in Vector2 other) => this - other;

        public static Vector2 Normalize(in Vector2 value)
        {
            var factor = 1f / DistanceTo(in value, in Zero);
            return value * factor;
        }
        public Vector2 Normalize() => Normalize(in this);

        #endregion

        #region Operators

        public static Vector2 operator -(in Vector2 a) => new Vector2(-a.X, -a.Y);
        public static Vector2 operator ++(in Vector2 a) => new Vector2(a.X, a.Y) + 1.0f;
        public static Vector2 operator --(in Vector2 a) => new Vector2(a.X, a.Y) - 1.0f;

        public static bool operator !=(in Vector2 a, in Vector2 b) => !a.Equals(in b);
        public static bool operator ==(in Vector2 a, in Vector2 b) => a.Equals(in b);
        public static bool operator >(in Vector2 a, in Vector2 b) => a.X > b.X && a.Y > b.Y;
        public static bool operator <(in Vector2 a, in Vector2 b) => a.X < b.X && a.Y < b.Y;
        public static bool operator >=(in Vector2 a, in Vector2 b) => a.X >= b.X && a.Y >= b.Y;
        public static bool operator <=(in Vector2 a, in Vector2 b) => a.X <= b.X && a.Y <= b.Y;

        public static Vector2 operator +(in Vector2 a, in Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(in Vector2 a, in Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(in Vector2 a, in Vector2 b) => new Vector2(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator /(in Vector2 a, in Vector2 b) => new Vector2(a.X / b.X, a.Y / b.Y);
        public static Vector2 operator %(in Vector2 a, in Vector2 b) => new Vector2(a.X % b.X, a.Y % b.Y);

        public static Vector2 operator +(in Vector2 a, float b) => new Vector2(a.X + b, a.Y + b);
        public static Vector2 operator -(in Vector2 a, float b) => new Vector2(a.X - b, a.Y - b);
        public static Vector2 operator *(in Vector2 a, float b) => new Vector2(a.X * b, a.Y * b);
        public static Vector2 operator /(in Vector2 a, float b) => new Vector2(a.X / b, a.Y / b);
        public static Vector2 operator %(in Vector2 a, float b) => new Vector2(a.X % b, a.Y % b);

        public static Vector2 operator +(float a, in Vector2 b) => new Vector2(a + b.X, a + b.Y);
        public static Vector2 operator -(float a, in Vector2 b) => new Vector2(a - b.X, a - b.Y);
        public static Vector2 operator *(float a, in Vector2 b) => new Vector2(a * b.X, a * b.Y);
        public static Vector2 operator /(float a, in Vector2 b) => new Vector2(a / b.X, a / b.Y);
        public static Vector2 operator %(float a, in Vector2 b) => new Vector2(a % b.X, a % b.Y);

        #endregion

        #region Constants

        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);

        #endregion

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != GetType())
                return false;

            if (obj is float f)
                return Equals(f);

            if (obj is Vector2 v2)
                return Equals(in v2);

            return false;
        }
        public bool Equals(float other) => other.Equals(X) && other.Equals(Y);
        public bool Equals(in Vector2 other) => other.X.Equals(X) && other.Y.Equals(Y);

        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
    }
}