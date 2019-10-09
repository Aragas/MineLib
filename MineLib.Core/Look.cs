using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MineLib.Core
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct Look : IEquatable<Look>
    {
        public readonly float Pitch;
        public readonly float Yaw;

        public Look(float pitch, float yaw) { Pitch = pitch; Yaw = yaw; }
        public Look(double pitch, double yaw) { Pitch = (float) pitch; Yaw = (float) yaw; }
        public Look(in Look look) { Pitch = look.Pitch; Yaw = look.Yaw; }
        public Look(in Vector3 standing, in Vector3 lookingAt)
        {
            var dx = lookingAt.X - standing.X;
            var dy = lookingAt.Y - standing.Y;
            var dz = lookingAt.Z - standing.Z;
            var r = MathF.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
            Yaw = -MathF.Atan2(dx, dz) / MathF.PI * 180;
            if (Yaw < 0F)
                Yaw += 360;
            Pitch = -MathF.Asin(dy / r) / MathF.PI * 180;
        }

        public Vector3 ToVector3() => new Vector3(
            -MathF.Cos(Pitch) * MathF.Sin(Yaw),
            -MathF.Sin(Pitch),
            MathF.Cos(Pitch) * MathF.Cos(Yaw));

        /// <summary>
        /// Converts this Rotation to a string.
        /// </summary>
        public override string ToString() => $"Pitch: {Pitch}, Yaw: {Yaw}";

        public static bool operator ==(in Look a, in Look b) => a.Pitch == b.Pitch && a.Yaw == b.Yaw;
        public static bool operator !=(in Look a, in Look b) => !(a == b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) => obj is Look rotation && Equals(rotation);
        public bool Equals(Look other) => other.Pitch.Equals(Pitch) && other.Yaw.Equals(Yaw);

        public override int GetHashCode() => HashCode.Combine(Yaw, Pitch);
    }
}